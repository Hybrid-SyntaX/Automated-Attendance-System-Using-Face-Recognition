using AAS.Proxy;
using AM.FRS.Contracts;
using AM.FRS.Util;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAS.Client.AttendanceMethods
{

    /// <summary>
    /// روش تشخیص چهره
    /// </summary>
    public class FaceRecognitionMethod : IAttendanceMethod
    {
        private IFaceRecognitionService FaceRecognition;
        private IList<DetectedObject> m_faces;
        private object input;
        private double m_scale = 0.5;

        /// <summary>
        /// متد سازنده
        /// </summary>
        public FaceRecognitionMethod()
        {
            FaceRecognition = new FaceRecognitionServiceProxy();
        }

        /// <summary>
        /// برسی وجود صورت در تصویر
        /// </summary>
        /// <returns>نتیجه برسی</returns>
        public bool Check()
        {
            Bitmap l_bitmap = EmguCVHelper.ScaleImage((Image<Bgr, byte>)Input, m_scale).ToBitmap();
            m_faces = this.FaceRecognition.DetectFace(l_bitmap);
            return m_faces.Count > 0;
        }

        /// <summary>
        /// شناسایی چهره
        /// </summary>
        /// <returns>شناسنه داخلی</returns>
        public object Run()
        {
            ///[Checking for face]
            string l_employeeIDString = string.Empty;

            //if (Check())
            //{
                if (m_faces.Count > 0)
                {
                    //Face recognition is run over all detected "faces" and the first valid occurance is retrned
                    foreach (DetectedObject face in m_faces)
                    {
                        l_employeeIDString = FaceRecognition.RecognizeFace(face);
                        if(!string.IsNullOrEmpty(l_employeeIDString))
                        {
                            break;
                        }
                    }
                    //m_faces.FirstOrDefault(face => !string.IsNullOrEmpty((l_employeeIDString = FaceRecognition.RecognizeFace(face))));
                }
       //     }
            ///[Checking for face]


            ///[Returning found Employee internal id]
            Guid l_employeeId = Guid.Empty;
            Guid.TryParse(l_employeeIDString, out l_employeeId);

            return l_employeeId;
            ///[Running face recognition]
        }

        /// <summary>
        /// تصویر ورودی
        /// </summary>
        public object Input
        {
            get
            {
                return input;
            }
            set
            {
                input = value;
            }
        }

        /// <summary>
        /// مشخص کردن صورت (های) تشخیص داهد شده
        /// </summary>
        /// <param name="input">تصویر ورودی</param>
        public void DrawFacesBoundaries(Image<Bgr, byte> input)
        {

            if (m_faces != null && m_faces.Count > 0)
                foreach (DetectedObject face in m_faces)
                {
                    if (face != null)
                    {

                        Rectangle rectangle =
                            new Rectangle(
                                face.Region.X * (int)(1 / m_scale) - (int)(face.Region.Height * 0.15),
                                face.Region.Y * (int)(1 / m_scale) - (int)(face.Region.Height * 0.15),
                                face.Region.Width * (int)(1 / m_scale) + (int)(face.Region.Height * 0.35),
                                face.Region.Height * (int)(1 / m_scale) + (int)(face.Region.Height * 0.35)
                            );

                        input.Draw(rectangle, new Bgr(255, 255, 255), 2);
                    }
                }

            //EmguCVHelper.CaptureImage.ToBitmap();
        }
    }
}
