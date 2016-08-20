using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Runtime.InteropServices;
//using System.Windows.Media.Imaging;
using System.Windows;
//using System.Windows.Threading;
//using System.Windows.Media;
using System.Threading;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using AM.FRS.Contracts;
using System.Drawing;

namespace AM.FRS.Util
{
    /// <summary>
    /// یک کلاس کمکی برای انجام اعمال مشترک پردازش تصویر
    /// </summary>
    public class EmguCVHelper
    {

        private static Capture m_capture;
        private static Image<Bgr, Byte> m_frame;
        private static DispatcherTimer m_timer;

        /// <summary>
        /// تصویر ورودی Refresh شده است
        /// </summary>
        public static event EventHandler CaptureImageIsRefreshed;

        /// <summary>
        /// شروع دریافت تصویر از دوربین
        /// </summary>
        public static void StopCapture()
        {
            ///[Stop Capturing]
            if (m_capture != null)
                m_capture.Dispose();
            ///[Stop Capturing]
        }

        /// <summary>
        /// اتمام دریافت تصویر از دوربین
        /// </summary>
        public static void StartCapture()
        {
            ///[Start Capturing]
            if (m_capture == null)
            {
                try
                {
                    m_capture = new Capture(0);

                }
                catch (NullReferenceException ex)
                {
                    throw ex;
                }
            }

            if (m_capture != null)
            {

                m_timer = new DispatcherTimer(TimeSpan.FromMilliseconds(1.0), DispatcherPriority.ContextIdle,
                    new EventHandler(processFrame), Application.Current.Dispatcher);
            }
            ///[Start Capturing]
        }

        /// <summary>
        /// دریافت فریم جاری
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void processFrame(object sender, EventArgs e)
        {
            ///[Stop capturing]
            m_frame = m_capture.QueryFrame();

            if (CaptureImageIsRefreshed != null)
                CaptureImageIsRefreshed(sender, e);
            ///[Stop capturing]
        }


        /// <summary>
        /// تغییر مقیاس تصویر
        /// </summary>
        /// <param name="image">تصویر ورودی</param>
        /// <param name="scale">مقیاس</param>
        /// <returns>تصویر با مقیاس جدید</returns>
        public static Image<Bgr, Byte> ScaleImage(Image<Bgr, byte> image, double scale)
        {
            ///[Scaling input image]
            return image.Resize((int)(image.Width * scale), (int)(image.Height * scale), Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            ///[Scaling input image]
        }

        /// <summary>
        /// دریافت فریم جاری در مقیاس تعیین شده
        /// </summary>
        /// <param name="scale">مقیاس</param>
        /// <returns>تصویر با مقیاس جدید</returns>
        public static Image<Bgr, Byte> GetScaledCaptureImage(double scale)
        {
            return ScaleImage(m_frame, scale);
        }

        /// <summary>
        //فریم جاری دریافت شده از دوربین
        /// </summary>
        public static Image<Bgr, Byte> CaptureImage
        {
            get
            {
                return m_frame;
            }
        }


        /// <summary>
        /// این متد، اشیای شناسایی شده در تصویر را روی تصویر علامت گذاری می کند
        /// </summary>
        /// <param name="input">تصویر ورودی</param>
        /// <param name="detectedObjects">لیست اشیای تشخیص داده شده</param>
        /// <param name="originalScale">مقیاس اصلی تصویر ورودی</param>
        public static void DrawObjectsBoundaries(Image<Bgr, byte> input, List<DetectedObject> detectedObjects, double originalScale)
        {
            ///[Drawing object boundaries]
            if (detectedObjects != null && detectedObjects.Count > 0)
                foreach (DetectedObject face in detectedObjects)
                {
                    if (face != null)
                    {

                        Rectangle rectangle =
                            new Rectangle(
                                face.Region.X * (int)(1 / originalScale) - (int)(face.Region.Height * 0.15),
                                face.Region.Y * (int)(1 / originalScale) - (int)(face.Region.Height * 0.15),
                                face.Region.Width * (int)(1 / originalScale) + (int)(face.Region.Height * 0.35),
                                face.Region.Height * (int)(1 / originalScale) + (int)(face.Region.Height * 0.35)
                            );

                        input.Draw(rectangle, new Bgr(255, 255, 255), 2);
                    }
                }
            ///[Drawing object boundaries]
        }

    }
}
