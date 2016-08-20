using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace AAS.GUI.Converters
{
    /// <summary>
    /// یک Converter برا تبدیل هر نوعی به یک ObjectValue
    /// </summary>
    /// <seealso cref="ObjectValue"/>
    public class ObjectValueConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// متد سازنده
        /// </summary>
        public ObjectValueConverter() { }

        /// <summary>
        /// تبدیل مقدار مورد نظر به ObjectValue
        /// </summary>
        /// <param name="value">مقداری ه قرار است تبدیل شود</param>
        /// <param name="targetType">یک ObjectValue</param>
        /// <param name="parameter">این پارامتر در ObjectValueConverter استفاده نمی شود</param>
        /// <param name="culture">این پارامتر در ObjectValueConverter استفاده نمی شود</param>
        /// <returns>یک ObjectValue حاوی مقدار موجود در Value</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new ObjectValue(value);
        }

        /// <summary>
        /// تبدیل ObjectValue به مقدار اصلی
        /// </summary>
        /// <param name="value">یک ObjectValue</param>
        /// <param name="targetType">نوع مقصد</param>
        /// <param name="parameter">این پارامتر در ObjectValueConverter استفاده نمی شود</param>
        /// <param name="culture">این پارامتر در ObjectValueConverter استفاده نمی شود</param>
        /// <returns>مقدار اصلی</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as ObjectValue).Value;
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
