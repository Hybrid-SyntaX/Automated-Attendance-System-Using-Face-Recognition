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
    /// تبدیل معکوس Boolean
    /// این Converter ابتدا مقدار را به Boolean تبدیل سپس آن را NOT می کند.
    /// </summary>
    /// <seealso cref="BooleanConverter"/>
    public class InverseBooleanConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// تبدیل به Boolean و NOT کردن آن.
        /// </summary>
        /// <param name="value">یک نوع قابل تبدیل به Boolean</param>
        /// <param name="targetType">نوع مقصد</param>
        /// <param name="parameter">این پارامتر در InverseBooleanConverter استفاده نمی شود</param>
        /// <param name="culture">این پارامتر در InverseBooleanConverter استفاده نمی شود</param>
        /// <returns>مقدار NOT شده</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
                return !((bool)value);

            bool? boolValue=null;
            try
            {
                boolValue = System.Convert.ToBoolean(value);

            }
            catch(Exception)
            {
                throw new InvalidOperationException("The target must be a boolean or convertable to boolean");
            }

            return !(bool)boolValue;
        }

        /// <summary>
        ///این عمل در InverseBooleanConverter پشتیبانی نمی شود
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
