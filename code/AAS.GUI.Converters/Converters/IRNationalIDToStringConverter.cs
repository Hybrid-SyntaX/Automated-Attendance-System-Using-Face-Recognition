using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAS.Entities;
using System.Windows.Data;
using System.Windows.Markup;
namespace AAS.GUI.Converters
{
    /// <summary>
    ///  تبدیل IRNationalID به رشته
    /// </summary>
    public class IRNationalIDToStringConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// متد سازنده
        /// </summary>
        public IRNationalIDToStringConverter()
        {

        }

        /// <summary>
        /// تبدیل IRNationalID به رشته متناظر
        /// </summary>
        /// <param name="value">یک نمونه IRNationalID</param>
        /// <param name="targetType">یک string</param>
        /// <param name="parameter">این پارامتر در IRNationalIDToStringConverter استفاده نمی شود</param>
        /// <param name="culture">این پارامتر در IRNationalIDToStringConverter استفاده نمی شود</param>
        /// <returns>یک رشته متانظر</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(string))
                return value.ToString();
            else return null;
        }

        /// <summary>
        /// تبدیل رشته به IRNationalID متناظر
        /// </summary>
        /// <param name="value">یک string</param>
        /// <param name="targetType">یک نمونه IRNationalID</param>
        /// <param name="parameter">این پارامتر در IRNationalIDToStringConverter استفاده نمی شود</param>
        /// <param name="culture">این پارامتر در IRNationalIDToStringConverter استفاده نمی شود</param>
        /// <returns>یک IRNationalID متناظر</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IRNationalID l_irnationalId = IRNationalID.Empty;
            if (targetType == typeof(IRNationalID))
            {
                if(!IRNationalID.TryParse((string)value,out l_irnationalId))
                {
                    l_irnationalId = IRNationalID.InvalidValue;
                }
            }
            return l_irnationalId;
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
