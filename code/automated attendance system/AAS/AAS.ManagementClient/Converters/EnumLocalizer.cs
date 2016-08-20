using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace AAS.ManagementClient.Converters
{
    /// <summary>
    /// یک Converter برا ترجمه مقادیر Enum موجود
    /// </summary>
    public class EnumLocalizer : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// متد سازنده
        /// </summary>
        public EnumLocalizer()
        {

        }

        /// <summary>
        /// ترجمه مقدار موجود در آرایه، یا تک مقدار
        /// </summary>
        /// <param name="value">مقداری که قرار است تبدیل شود</param>
        /// <param name="targetType">نوع مقد</param>
        /// <param name="parameter">این پارامتر در EnumLocalizer استفاده نمی شود</param>
        /// <param name="culture">این پارامتر در EnumLocalizer استفاده نمی شود</param>
        /// <returns>یک آرایه یا یک رشته</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is object[])
            {
                object[] multiResult;

                object[] array = (object[])value;
                multiResult = new object[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    multiResult[i] = new { Text = (string)Application.Current.TryFindResource(array[i]), Value = array[i] };
                }

                return multiResult;
            }
            else
            {
                if ((string)parameter == "StringOnly")
                    return Application.Current.TryFindResource(value.ToString());
                else return new { Text = Application.Current.TryFindResource(value), Value = value };
            }
        }
        /// <summary>
        /// این عمل در EnumLocalizer پشتیبانی نمی شود
        /// </summary>
        ///<exception cref="NotSupportedException" />
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
