using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Drawing;
namespace AM.FRS.Contracts
{

    [ServiceContract]
    public interface IFaceRecognitionService
    {
        /// <summary>
        /// تشخیص صورت
        /// </summary>
        /// <param name="bitmap">تصویر حاوی صورت</param>
        /// <returns>یک لیست از صورت های تشخیص داهد شده</returns>
        [OperationContract]
        List<DetectedObject> DetectFace(Bitmap bitmap);

        /// <summary>
        /// شناسایی چهره
        /// </summary>
        /// <param name="bitmap">تصویر حاوی صورت</param>
        /// <returns>شناسه</returns>
        [OperationContract(Name="RecognizeFaceByBitmap")]
        string RecognizeFace(Bitmap bitmap);

        /// <summary>
        /// شناسایی چهره
        /// </summary>
        /// <param name="face">صورت تشخیص داده شده به نوع <c>DetectedObject</c></param>
        /// <returns>شناسه</returns>
        [OperationContract(Name = "RecognizeFaceByDetectedObject")]
        string RecognizeFace(DetectedObject face);

        /// <summary>
        /// ذخیره چهره
        /// </summary>
        /// <param name="bitmap">تصویر چهره</param>
        /// <param name="id">شناسه خارجی</param>
        [OperationContract]
        void SaveFace(Bitmap bitmap, string id);

        /// <summary>
        /// حذف چهره
        /// </summary>
        /// <param name="id">شناسه خارجی</param>
        [OperationContract]
        void DeleteFace(string id);

        /// <summary>
        /// ذخیره سازی تنظیمات
        /// </summary>
        /// <param name="settings"></param>
        [OperationContract]
        void SetSettings(FRSSettings settings);

        /// <summary>
        /// دریافت تنظیمات
        /// </summary>
        /// <param name="getDefaultSettings">اگر مقدار true باشد، تنظیمات پیش فرض برگردانده می شوند.</param>
        /// <returns></returns>
        [OperationContract]
        FRSSettings GetSettings(bool getDefaultSettings);
    }
}
