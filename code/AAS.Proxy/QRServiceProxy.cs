using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using AM.QRService.Contracts;
using System.Drawing;
namespace AAS.Proxy
{
    /// <summary>
    /// پراکسی سرویس QRService
    /// </summary>
    public class QRServiceProxy:ClientBase<IQRService> , IQRService
    {
        /// <summary>
        /// رمزگشایی یک کد QR
        /// </summary>
        /// <param name="bitmapBarcode">تصویر حاوی کد QR</param>
        /// <returns>مقدار موجود در کد QR</returns>
        public string Read(Bitmap barcodeBitmap)
        {
            return Channel.Read(barcodeBitmap);
        }

        /// <summary>
        /// تبدیل یک رشته به یک کد QR
        /// </summary>
        /// <param name="contents">رشته مورد نظر</param>
        /// <returns>تصویر کد QR</returns>
        public Bitmap Write(string contents)
        {
            return Channel.Write(contents);
        }

        /// <summary>
        /// برسی وجود کد QR در تصویر
        /// </summary>
        /// <param name="bitmap">تصویر مورد نظر</param>
        /// <returns>نتیجه برسی</returns>
        public bool ContainsQRCode(Bitmap bitmap)
        {
            return Channel.ContainsQRCode(bitmap);
        }

        /// <summary>
        /// برسی وجود کد QR در تصویر
        /// </summary>
        /// <param name="bitmapByteArray">تصویر مورد نظر در قالب آرایه ای از <c>byte</c></param>
        /// <param name="width">عرض تصویر به پیکسل</param>
        /// <param name="height">طور تصویر به پیکسل</param>
        /// <returns>نتیجه برسی</returns>
        public bool ContainsQRCode(byte[] bitmapByteArray, int width, int height)
        {
            return Channel.ContainsQRCode(bitmapByteArray, width, height);
        }
    }
}
