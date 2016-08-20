using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM.FRS.Contracts;
using System.Drawing;
using System.ServiceModel;
using Emgu.CV;
using Emgu.CV.Structure;
using AM.FRS.Properties;
using System.IO;
using System.Xml.Linq;
using System.Drawing.Imaging;
using System.IO.Compression;
namespace AM.FRS
{
    /// <summary>
    /// سرویس تشخیص چهره
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class FaceRecognitionService : IFaceRecognitionService
    {
        private CascadeClassifier m_cascadeClassifier;
        private FaceRecognizer m_faceRecognizer;
        private FRSSettings FRSSettings;

        /// <summary>
        /// تمرین تشخیص چهره
        /// </summary>
        private void trainFaceRecognizer()
        {
            ///[Traitning Face Recognizer]
            List<Image<Gray, byte>> images = new List<Image<Gray, byte>>();
            List<int> labels = new List<int>();

            FaceRepository.OpenFacesDatabase(ref images, ref labels);

            if (images.Count > 0 && labels.Count > 0)
                m_faceRecognizer.Train(images.ToArray(), labels.ToArray());
            ///[Traitning Face Recognizer]

        }

        /// <summary>
        /// صورت را از فریم جاری جدا می کند
        /// </summary>
        /// <param name="grayFrame">فریم جاری در قالب Grayscale</param>
        /// <param name="face">محدوده تشخیص داده شده به عنوان صورت</param>
        /// <returns>تصویر صورت استخراج شده را بر می گرداند</returns>
        private DetectedObject extractFace(Image<Gray, byte> grayFrame, Rectangle face)
        {

            ///[Defining face regions]
            face.X += (int)(face.Height * 0.15);
            face.Y += (int)(face.Width * 0.22);
            face.Height -= (int)(face.Height * 0.3);
            face.Width -= (int)(face.Width * 0.35);
            ///[Defining face regions]

            ///[Face is resized and equalized]

            try
            {


                Image<Gray, byte> result = grayFrame.Copy(face).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                result._EqualizeHist();

                ///[Face is resized and equalized]


                ///[Saving extracted and equalized face]
                Bitmap l_extractedFace = null;
                Graphics l_faceCanvas;

                l_extractedFace = new Bitmap(100, 100);
                l_faceCanvas = Graphics.FromImage(l_extractedFace);
                l_faceCanvas.DrawImage(result.ToBitmap(), new Point(0, 0));

                DetectedObject l_detectedObject = new DetectedObject(l_extractedFace, face);
                ///[Saving extracted and equalized face]
                return l_detectedObject;
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// متد سازنده <c>FaceRecognitionService</c>
        /// </summary>
        public FaceRecognitionService()
        {
            m_cascadeClassifier = new CascadeClassifier(@"data/haarcascade_frontalface_default.xml");

            ///[Initializing EigenFaceRecognizer]
            double l_threshold;
            if (Settings.Default.EigenFaceRecognizerThresholdInfinity)
                l_threshold = double.PositiveInfinity;
            else
                l_threshold = Settings.Default.EigenFaceRecognizerThreshold;
            m_faceRecognizer = new EigenFaceRecognizer(Settings.Default.EigenFaceRecognizerNumComponents, l_threshold);


            trainFaceRecognizer();
            ///[Initializing EigenFaceRecognizer]
            Settings.Default.EigenFaceRecognizerThreshold = 2;

            ///[Initializing Settings]
            this.FRSSettings = new FRSSettings()
            {
                CascadeClassifierMaxSize = Settings.Default.CascadeClassifierMaxSize,
                CascadeClassifierMinSize = Settings.Default.CascadeClassifierMinSize,
                CascadeClassifierScaleFactor = Settings.Default.CascadeClassifierScaleFactor,
                CascadeClassifierMinNeighbours = Settings.Default.CascadeClassifierMinNeighbours,
                EigenFaceRecognizerThreshold = Settings.Default.EigenFaceRecognizerThreshold,
                EigenFaceRecognizerThresholdInfinity = Settings.Default.EigenFaceRecognizerThresholdInfinity,
                EigenFaceRecognizerNumComponents = Settings.Default.EigenFaceRecognizerNumComponents,

            };
            ///[Initializing Settings]
        }

        /// <summary>
        /// تشخیص صورت
        /// </summary>
        /// <param name="bitmap">تصویر حاوی صورت</param>
        /// <returns>یک لیست از صورت های تشخیص داهد شده</returns>
        public List<DetectedObject> DetectFace(Bitmap bitmap)
        {
            Console.WriteLine("[{0}] Detecting face...", DateTime.Now.ToString("HH:mm:ss"));
            ///[Detecting face]
            Image<Bgr, byte> image = new Image<Bgr, byte>(bitmap);

            List<DetectedObject> l_detectedObjects = new List<DetectedObject>();
            if (image != null)
            {
                ///[Detecting faces]
                Image<Gray, byte> grayFrame = image.Convert<Gray, byte>();

                Rectangle[] faces = m_cascadeClassifier.DetectMultiScale(
                    grayFrame,
                    Settings.Default.CascadeClassifierScaleFactor,
                    Settings.Default.CascadeClassifierMinNeighbours,
                    Settings.Default.CascadeClassifierMinSize,
                    Settings.Default.CascadeClassifierMaxSize
                );
                ///[Detecting faces]
                Console.WriteLine("[{0}] {1} face(s) were detected", DateTime.Now.ToString("HH:mm:ss"), faces.Length);
                ///[Generating return value]
                if (faces.Length > 0)
                {
                    Parallel.ForEach(faces, face =>
                    {
                        DetectedObject l_detectedObject = extractFace(grayFrame, face);
                        if (l_detectedObject != null)
                            l_detectedObjects.Add(l_detectedObject);

                    });

                    bitmap = image.ToBitmap();
                    return l_detectedObjects;
                }
                ///[Generating return value]

            }
            return l_detectedObjects;
            ///[Detecting face]
        }

        /// <summary>
        /// شناسایی چهره
        /// </summary>
        /// <param name="bitmap">تصویر حاوی صورت</param>
        /// <returns>شناسه</returns>
        public string RecognizeFace(Bitmap bitmap)
        {
            Console.WriteLine("[{0}] Recognizing face...", DateTime.Now.ToString("HH:mm:ss"));
            ///[Recognizing face]
            FaceRecognizer.PredictionResult predicitonResult = m_faceRecognizer.Predict(new Image<Gray, byte>(bitmap));
            if (predicitonResult.Label == -1)
                return string.Empty;

            string id = FaceRepository.GetId(predicitonResult.Label);
            ///[Recognizing face]
            if (!string.IsNullOrEmpty(id))
                Console.WriteLine("[{0}] face id {1} was recognized...", DateTime.Now.ToString("HH:mm:ss"), id);
            return id;
        }

        /// <summary>
        /// شناسایی چهره
        /// </summary>
        /// <param name="face">صورت تشخیص داده شده به نوع <c>DetectedObject</c></param>
        /// <returns>شناسه</returns>
        public string RecognizeFace(DetectedObject face)
        {
            return RecognizeFace(face.Bitmap);
        }

        /// <summary>
        /// ذخیره چهره
        /// </summary>
        /// <param name="bitmap">تصویر چهره</param>
        /// <param name="id">شناسه خارجی</param>
        public void SaveFace(Bitmap bitmap, string id)
        {
            FaceRepository.SaveFaceBitmap(bitmap, id);
        }

        /// <summary>
        /// حذف چهره
        /// </summary>
        /// <param name="id">شناسه خارجی</param>
        public void DeleteFace(string id)
        {
            FaceRepository.DeleteFace(id);
        }

        /// <summary>
        /// ذخیره سازی تنظیمات
        /// </summary>
        /// <param name="settings"></param>
        public void SetSettings(FRSSettings settings)
        {
            ///[Set settings]
            this.FRSSettings = settings;
            Settings.Default.EigenFaceRecognizerThreshold = settings.EigenFaceRecognizerThreshold;
            Settings.Default.EigenFaceRecognizerThresholdInfinity = settings.EigenFaceRecognizerThresholdInfinity;
            Settings.Default.EigenFaceRecognizerNumComponents = settings.EigenFaceRecognizerNumComponents;
            Settings.Default.CascadeClassifierMaxSize = settings.CascadeClassifierMaxSize;
            Settings.Default.CascadeClassifierMinNeighbours = settings.CascadeClassifierMinNeighbours;
            Settings.Default.CascadeClassifierScaleFactor = settings.CascadeClassifierScaleFactor;
            Settings.Default.CascadeClassifierMinSize = settings.CascadeClassifierMinSize;
            Settings.Default.Save();
            ///[Set settings]
        }

        /// <summary>
        /// دریافت تنظیمات
        /// </summary>
        /// <param name="getDefaultSettings">اگر مقدار true باشد، تنظیمات پیش فرض برگردانده می شوند.</param>
        /// <returns></returns>
        public FRSSettings GetSettings(bool getDefaultSettings = false)
        {
            ///[Get settings]
            if (getDefaultSettings)
                return new FRSSettings()
                {
                    CascadeClassifierMaxSize = Settings.Default.DefaultCascadeClassifierMaxSize,
                    CascadeClassifierMinSize = Settings.Default.DefaultCascadeClassifierMinSize,
                    CascadeClassifierScaleFactor = Settings.Default.DefaultCascadeClassifierScaleFactor,
                    CascadeClassifierMinNeighbours = Settings.Default.DefaultCascadeClassifierMinNeighbours,
                    EigenFaceRecognizerThreshold = Settings.Default.EigenFaceRecognizerThreshold,
                    EigenFaceRecognizerThresholdInfinity = Settings.Default.DefaultEigenFaceRecognizerThresholdInfinity,
                    EigenFaceRecognizerNumComponents = Settings.Default.DefaultEigenFaceRecognizerNumComponents,

                };
            else return FRSSettings;
            ///[Get settings]
        }
    }
}
