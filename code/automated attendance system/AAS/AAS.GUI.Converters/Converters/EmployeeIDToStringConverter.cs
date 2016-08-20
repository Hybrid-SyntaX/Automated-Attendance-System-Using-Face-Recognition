using AAS.Entities;
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
    /// تبدیل EmployeeID به رشته
    /// </summary>
    public class EmployeeIDToStringConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// متد سازنده
        /// </summary>
        public EmployeeIDToStringConverter()
        {

        }

        /// <summary>
        /// تبدیل EmployeeID به رشته
        /// </summary>
        /// <param name="value">یک EmployeeID</param>
        /// <param name="targetType">یک string</param>
        /// <param name="parameter">این پارامتر در EmployeeIDToStringConverter استفاده نمی شود</param>
        /// <param name="culture">این پارامتر در EmployeeIDToStringConverter استفاده نمی شود</param>
        /// <returns>یک رشته</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(string) && value!=null)
                return value.ToString();
            else return null;
        }

        /// <summary>
        /// تبدیل رشته به EmployeeID متناظر
        /// </summary>
        /// <param name="value">یک string</param>
        /// <param name="targetType">یک EmployeeID</param>
        /// <param name="parameter">این پارامتر در EmployeeIDToStringConverter استفاده نمی شود</param>
        /// <param name="culture">این پارامتر در EmployeeIDToStringConverter استفاده نمی شود</param>
        /// <returns>نمونه متناظر EmployeeID</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (targetType == typeof(EmployeeID) && value !=null)
                return EmployeeID.Parse((string)value);
            else return null;
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
