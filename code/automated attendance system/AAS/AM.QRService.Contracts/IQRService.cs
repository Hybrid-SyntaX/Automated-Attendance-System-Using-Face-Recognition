using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Drawing;
namespace AM.QRService.Contracts
{
    [ServiceContract]
    public interface IQRService
    {
        [OperationContract]
        string Read(Bitmap barcodeBitmap);

        [OperationContract]
        Bitmap Write(string contents);

        [OperationContract(Name="ContainsQRCodeByBitmap")]
        bool ContainsQRCode(Bitmap bitmap);

        [OperationContract(Name="ContainsQRCodeByByteWidthHeight")]
        bool ContainsQRCode(byte [] bitmapByteArray, int width, int height);
    }
}
