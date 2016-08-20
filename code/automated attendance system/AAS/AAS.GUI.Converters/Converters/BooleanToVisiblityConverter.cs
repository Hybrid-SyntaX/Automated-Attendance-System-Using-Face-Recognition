using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace AAS.GUI.Converters
{
    /// <summary>
    /// یک Converter برای تبدیل <c>Boolean</c> به <c>Visiblity</c>
    /// </summary>
    public class BooleanToVisiblityConverter : MarkupExtension, IValueConverter
    {

        /// <summary>
        /// تبدیل <c>Boolean</c> به <c>Visiblity</c>
        /// </summary>
        /// <param name="value">مقدار boolean</param>
        /// <param name="targetType">یک Visiblity</param>
        /// <param name="parameter">اگر مقدار "Inverse" داشته باشد، عمل NOT روی مقدار Boolean انجام می شود.</param>
        /// <param name="culture">این پارامتر در BooleanToVisiblityConverter استفاده نمی شود</param>
        /// <returns>مقدار Visiblity</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isInverse = (string)parameter == "Inverse";
            
            
            bool boolValue = (bool)value;
            Visibility result = (isInverse) ? Visibility.Collapsed : Visibility.Visible;
            if (targetType == typeof(Visibility))
            {
                if (isInverse)
                    result = (boolValue) ? Visibility.Collapsed : Visibility.Visible;
                else result = (boolValue) ? Visibility.Visible : Visibility.Collapsed;
            }

            return result;
        }

        /// <summary>
        /// تبدیل <c>Visiblity</c> به <c>Boolean</c>
        /// </summary>
        /// <param name="value">مقدار Visiblity</param>
        /// <param name="targetType">یک boolean</param>
        /// <param name="parameter">اگر مقدار "Inverse" داشته باشد، عمل NOT روی مقدار Boolean انجام می شود.</param>
        /// <param name="culture">این پارامتر در BooleanToVisiblityConverter داستفاده نمی شود</param>
        /// <returns>مقدار Boolean</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isInverse = (string)parameter == "Inverse";

            Visibility visiblityValue = (Visibility)value;
            bool result = true;
            if (targetType == typeof(bool))
            {
                result = visiblityValue == Visibility.Visible;
            }

            return isInverse ? !result : result;
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
