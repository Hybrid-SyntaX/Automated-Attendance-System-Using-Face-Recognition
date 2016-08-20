using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM.QRService.Contracts;
using AAS.Proxy;
using AM.FRS.Util;
namespace AAS.Client.AttendanceMethods
{
    /// <summary>
    /// اعلام حضور با روش QR
    /// </summary>
    public class QRMethod : IAttendanceMethod
    {
        IQRService QrService;
        
        /// <summary>
        /// متد سازنده
        /// </summary>
        public QRMethod()
        {
            QrService = new QRServiceProxy();
        }

        private object input;
        /// <summary>
        /// ورودی
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
        /// برسی وجود کد QR
        /// </summary>
        /// <returns>نتیجه برسی</returns>
        public bool Check()
        {
            Image<Bgr, byte> image = (Image<Bgr, byte>)Input;
            if (image == null)
                return false;
            else return this.QrService.ContainsQRCode(image.Bytes, image.Width, image.Height);
        }

        /// <summary>
        /// رمزگشایی کد QR موجود
        /// </summary>
        /// <returns>مقدار رمزگشایی شده</returns>
        public object Run()
        {
            string l_IDString  = (string)QrService.Read(EmguCVHelper.ScaleImage((Image<Bgr, byte>)Input, 0.25).ToBitmap());

            Guid l_Id = Guid.Empty;
            Guid.TryParse(l_IDString, out l_Id);

            return l_Id;
        }
    }
}
