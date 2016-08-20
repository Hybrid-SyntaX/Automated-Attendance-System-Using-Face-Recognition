using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM.QRService.Contracts;
using ZXing;
using System.Drawing;
using ZXing.Common;
using System.ServiceModel;
namespace AM.QRService
{
    /// <summary>
    /// کلاس سرویس QR
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class QRService : IQRService
    {
        private BarcodeReader m_barcodeReader;
        private BarcodeWriter m_barcodeWriter;

        /// <summary>
        /// متد سازنده <c>QRService</c>
        /// </summary>
        public QRService()
        {
            ///[Initializing QR]
            m_barcodeReader = new BarcodeReader();
            m_barcodeWriter = new BarcodeWriter() { Format = BarcodeFormat.QR_CODE };
            ///[Initializing QR]
        }

        /// <summary>
        /// رمزگشایی یک کد QR
        /// </summary>
        /// <param name="bitmapBarcode">تصویر حاوی کد QR</param>
        /// <returns>مقدار موجود در کد QR</returns>
        public string Read(Bitmap bitmapBarcode)
        {
            ///[Decoding QR Code]
            
            Result result = m_barcodeReader.Decode(bitmapBarcode);
            if (result == null)
                return null;
            return result.Text;

            ///[Decoding QR Code]
        }

        /// <summary>
        /// تبدیل یک رشته به یک کد QR
        /// </summary>
        /// <param name="contents">رشته مورد نظر</param>
        /// <returns>تصویر کد QR</returns>
        public Bitmap Write(string contents)
        {
            ///[Encoding to QR Code]
            return m_barcodeWriter.Write(contents);
            ///[Encoding to QR Code]
        }

        /// <summary>
        /// برسی وجود کد QR در تصویر
        /// </summary>
        /// <param name="bitmap">تصویر مورد نظر</param>
        /// <returns>نتیجه برسی</returns>
        public bool ContainsQRCode(Bitmap bitmap)
        {
            ///[Checking picture for QR Code with Bitmap input]
            ImageConverter converter = new ImageConverter();
            byte[] bitmapBytes = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));

            return ContainsQRCode(bitmapBytes, bitmap.Width, bitmap.Height);
            ///[Checking picture for QR Code with Bitmap input]
            
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
            ///[Checking picture for QR Code]
            ZXing.LuminanceSource source = new ZXing.RGBLuminanceSource(bitmapByteArray, width, height);
            ZXing.Common.HybridBinarizer binarizer = new ZXing.Common.HybridBinarizer(source);
            ZXing.BinaryBitmap binBitmap = new ZXing.BinaryBitmap(binarizer);
            ZXing.Common.BitMatrix bitMatrix = binBitmap.BlackMatrix;
            
            ZXing.Multi.QrCode.Internal.MultiDetector detector = new ZXing.Multi.QrCode.Internal.MultiDetector(bitMatrix);

            return detector.detect() != null;
            ///[Checking picture for QR Code]
        }
    }
}
