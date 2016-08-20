using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM.FRS.Contracts;
using System.ServiceModel;
using System.Drawing;
namespace AAS.Proxy
{
    /// <summary>
    /// پراکسی سرویس FaceRecognitionService
    /// </summary>
    public class FaceRecognitionServiceProxy : ClientBase<IFaceRecognitionService>, IFaceRecognitionService
    {
        /// <summary>
        /// تشخیص صورت
        /// </summary>
        /// <param name="bitmap">تصویر حاوی صورت</param>
        /// <returns>یک لیست از صورت های تشخیص داهد شده</returns>
        public List<DetectedObject> DetectFace(Bitmap bitmap)
        {
            return Channel.DetectFace(bitmap);
        }

        /// <summary>
        /// شناسایی چهره
        /// </summary>
        /// <param name="bitmap">تصویر حاوی صورت</param>
        /// <returns>شناسه</returns>
        public string RecognizeFace(Bitmap bitmap)
        {
            return Channel.RecognizeFace(bitmap);
        }

        /// <summary>
        /// ذخیره چهره
        /// </summary>
        /// <param name="bitmap">تصویر چهره</param>
        /// <param name="id">شناسه خارجی</param>
        public void SaveFace(Bitmap bitmap, string id)
        {
            Channel.SaveFace(bitmap, id);
        }

        /// <summary>
        /// شناسایی چهره
        /// </summary>
        /// <param name="face">صورت تشخیص داده شده به نوع <c>DetectedObject</c></param>
        /// <returns>شناسه</returns>
        public string RecognizeFace(DetectedObject face)
        {
            return Channel.RecognizeFace(face);
        }

        /// <summary>
        /// حذف چهره
        /// </summary>
        /// <param name="id">شناسه خارجی</param>
        public void DeleteFace(string id)
        {
             Channel.DeleteFace(id);
        }

        /// <summary>
        /// ذخیره سازی تنظیمات
        /// </summary>
        /// <param name="settings"></param>
        public void SetSettings(FRSSettings settings)
        {
            Channel.SetSettings(settings);
        }

        /// <summary>
        /// دریافت تنظیمات
        /// </summary>
        /// <param name="getDefaultSettings">اگر مقدار true باشد، تنظیمات پیش فرض برگردانده می شوند.</param>
        /// <returns></returns>
        public FRSSettings GetSettings(bool getDefaultSettings=false)
        {
            return Channel.GetSettings(getDefaultSettings);
        }
    }
}
