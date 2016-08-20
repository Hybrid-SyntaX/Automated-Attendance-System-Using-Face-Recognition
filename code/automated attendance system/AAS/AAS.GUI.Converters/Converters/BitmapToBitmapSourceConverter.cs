using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AAS.GUI.Converters
{

    /// <summary>
    /// یک Converter برای تبدیل <c>Bitmap</c> به <c>BitmapSource</c> جهت نمایش در WPF
    /// </summary>
    public class BitmapToBitmapSourceConverter: MarkupExtension, IValueConverter
    {
        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        /// <summary>
        /// متد سازنده
        /// </summary>
        public BitmapToBitmapSourceConverter()
        {

        }

        /// <summary>
        /// تبدیل <c>Bitmap</c> به <c>BitmapSource</c>
        /// </summary>
        /// <param name="value">مقداری که قرار  است تبدیل شود</param>
        /// <param name="targetType">نوع مقصد</param>
        /// <param name="parameter">این پارامتر در BitmapToBitmapSourceConverter استفاده نمی شود</param>
        /// <param name="culture">این پارامتر در BitmapToBitmapSourceConverter داستفاده نمی شود</param>
        /// <returns>نمونه تبدیل شده به <c>BitmapSource</c></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(ImageSource))
                return toBitmapSource((Bitmap)value);
            else return null;
        }

        /// <summary>
        ///این عمل در <c>BooleanConverter</c> پشتیبانی نمی شود
        /// </summary>
        /// <exception cref="NotSupportedException"  />
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// مقداری که در XAML نمایش داده می شود
        /// </summary>
        /// <param name="serviceProvider">ارائه خدمات برای MarkupExtension</param>
        /// <returns>مقداری که در هنگام استفاده از این Converter به Property مورد نظر داده می شود</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        /// <summary>
        /// متد تبدیل <c>Bitmap</c> به <c>BitmapSource</c>
        /// </summary>
        /// <param name="bitmap">Bitmap که قرار است تبدیل شود</param>
        /// <returns>BitmapSource متناظر</returns>
        private static BitmapSource toBitmapSource(System.Drawing.Bitmap bitmap)
        {
            ///[Converting Bitmap to BitmapSource]
            if (bitmap == null)
                return null;
            IntPtr ptr = bitmap.GetHbitmap(); //obtain the Hbitmap

            BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                ptr,
                IntPtr.Zero,
                Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(ptr); //release the HBitmap
            return bs;
            ///[Converting Bitmap to BitmapSource]
        }
    }
}
