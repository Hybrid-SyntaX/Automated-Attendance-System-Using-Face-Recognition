using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
namespace AAS.GUI.Converters
{
    /// <summary>
    /// یک Converter برای تبدیل تمام مقادیر قابل تبدیل به Boolean
    /// </summary>
    public class BooleanConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// متد سازنده
        /// </summary>
        public BooleanConverter()
        {

        }

        /// <summary>
        /// تبدیل تمام مقادیر قابل تبدیل به Boolean
        /// </summary>
        /// <param name="value">مقداری که قرار  است تبدیل شود</param>
        /// <param name="targetType">نوع مقصد</param>
        /// <param name="parameter">این پارامتر در BooleanConverter استفاده نمی شود</param>
        /// <param name="culture">این پارامتر در BooleanConverter داستفاده نمی شود</param>
        ///<returns>مقدار boolean</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool? boolValue=null;
            try
            {
                boolValue = System.Convert.ToBoolean(value);

            }
            catch(Exception)
            {
                throw new InvalidOperationException("The target must be a boolean or convertable to boolean");
            }

            return (bool)boolValue;
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
    }
}
