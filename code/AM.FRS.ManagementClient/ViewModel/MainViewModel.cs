using System.Collections.Generic;
using GalaSoft.MvvmLight;
using System.Windows.Media;
using System.Windows.Threading;
using AM.FRS.Util;
using System.Runtime.InteropServices;
using System;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Emgu.CV;
using AAS.Proxy;
using System.Collections.ObjectModel;
using System.Drawing;
using Emgu.CV.Structure;
using System.Threading;
using System.Threading.Tasks;
namespace AM.FRS.ManagementClient.ViewModel
{

    /// <summary>
    /// MainViewModel
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public string Key { set; get; }
     
        /// <summary>
        /// متد سازنده
        /// </summary>
        public MainViewModel()
        {
            if (!IsInDesignMode)
            {
                try
                {
                    FaceRecognitionService = new FaceRecognitionServiceProxy();
                    if (FaceRecognitionService != null)
                    {


                        EmguCVHelper.StartCapture();
                        EmguCVHelper.CaptureImageIsRefreshed += RefreshImage;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            m_savedFrames = new List<Bitmap>();
            processCommandLineArgs();
        }

        /// <summary>
        /// پردازش مقادیر خط فرمان
        /// </summary>
        private void processCommandLineArgs()
        {
            if (Environment.GetCommandLineArgs().Length > 1)
            {

                string[] keyArgArray = Environment.GetCommandLineArgs()[1].Split('=');
                if (keyArgArray[0] == "key")
                    Key = keyArgArray[1];
            }
        }
        private FaceRecognitionServiceProxy FaceRecognitionService;

        private Bitmap m_detectedFace;

        /// <summary>
        /// صورت شناسایی شده؟
        /// </summary>
        public bool IsFaceDetected { set; get; }

        /// <summary>
        /// اندیس عکس انتخاب شده
        /// </summary>
        public int SelectedIndex { set; get; }

        
        private void RefreshImage(object sender, System.EventArgs e)
        {
            IsFaceDetected = false;

            Bitmap face = null;
            Bitmap bigImage = null;
            detectFace(out face, out bigImage);

            CaptureImage = EmguCVHelper.CaptureImage.ToBitmap();
        }
        private void detectFace(out System.Drawing.Bitmap face, out Bitmap bigImage)
        {
            face = null;
            double scale = 0.5;
            Bitmap bitmap = EmguCVHelper.GetScaledCaptureImage(scale).ToBitmap();
            
            var faces = FaceRecognitionService.DetectFace(bitmap);
            if (faces.Count > 0)
            {
                IsFaceDetected = true;

                face = bitmap;
                m_detectedFace = faces[0].Bitmap;

                EmguCVHelper.DrawObjectsBoundaries(EmguCVHelper.CaptureImage, faces, scale);
            }
            else IsFaceDetected = false;

            RaisePropertyChanged(() => IsFaceDetected);

            bigImage = EmguCVHelper.CaptureImage.ToBitmap();
        }

        ~MainViewModel()
        {
            EmguCVHelper.StopCapture();
        }

        private Bitmap m_captureImage;
        /// <summary>
        /// فریم جاری
        /// </summary>
        public Bitmap CaptureImage
        {
            private set
            {
                m_captureImage = value;
                RaisePropertyChanged(() => CaptureImage);
            }
            get
            {
                return m_captureImage;
            }
        }

        private List<Bitmap> m_savedFrames;

        /// <summary>
        /// عکس های گرفتهش ده
        /// </summary>
        public ObservableCollection<Bitmap> SavedFrames
        {
            get
            {
                return  new ObservableCollection<Bitmap>(m_savedFrames);
            }
        }


        void addImage()
        {
            m_savedFrames.Add(m_detectedFace);
            RaisePropertyChanged(() => SavedFrames);
        }

        private ICommand m_RemoveSelectedSavedFrameCommand;
        /// <summary>
        /// حذف فریم انتخاب شده
        /// </summary>
        public ICommand RemoveSelectedSavedFrameCommand
        {
            get
            {
                if (m_RemoveSelectedSavedFrameCommand == null)
                {
                    m_RemoveSelectedSavedFrameCommand = new RelayCommand(() =>
                    {

                        m_savedFrames.RemoveAt(SelectedIndex);
                        RaisePropertyChanged(() => SavedFrames);
                    },
                    () => m_savedFrames.Count > 0 && SelectedIndex != -1
                    );
                }
                return m_RemoveSelectedSavedFrameCommand;
            }
        }
        
        private ICommand m_AddFrameCommand;
       /// <summary>
       /// اضافه کردن فریم
       /// </summary>
        public ICommand AddFrameCommand
        {
            get
            {
                if (m_AddFrameCommand == null)
                    m_AddFrameCommand = new RelayCommand(addImage);
                return m_AddFrameCommand;
            }
        }

        private ICommand m_SaveFramesCommand;
        /// <summary>
        /// ذخیره تصاویر صورت استخراج شده در پایگاه داده چهره ها
        /// </summary>
        public ICommand SaveFramesCommand
        {
            get
            {

                if (m_SaveFramesCommand == null)
                {
                    m_SaveFramesCommand = new RelayCommand(() =>
                    {
                        foreach (Bitmap frame in m_savedFrames)
                        {
                            FaceRecognitionService.SaveFace(frame, Key);
                        }
                        m_savedFrames.Clear();
                        RaisePropertyChanged(() => SavedFrames);
                    }, () => m_savedFrames != null && m_savedFrames.Count > 0);
                }
                return m_SaveFramesCommand;
            }
        }
    }
}