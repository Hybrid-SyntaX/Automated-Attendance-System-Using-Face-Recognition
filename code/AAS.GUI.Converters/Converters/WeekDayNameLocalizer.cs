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
    /// یک Converter برای نمایش روزهای هفته به زبان فارسی
    /// </summary>
    public class WeekDayNameLocalizer : MarkupExtension, IValueConverter
    {
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
        /// متد سازنده
        /// </summary>
        public WeekDayNameLocalizer()
        {

        }
        /// <summary>
        /// تبدیل روز های هفته از زبان انگلیسی به فارسی
        /// </summary>
        /// <param name="value">یک DayOfWeek</param>
        /// <param name="targetType">نوع مقصد</param>
        /// <param name="parameter">این پارامتر در WeekDayNameLocalizer استفاده نمی شود</param>
        /// <param name="culture">این پارامتر در WeekDayNameLocalizer استفاده نمی شود</param>
        /// <returns>روز هفته به فارسی</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            switch((DayOfWeek)value)
            {
                case DayOfWeek.Saturday: return DaysInWeek.Saturday;
                case DayOfWeek.Sunday: return DaysInWeek.Sunday;
                case DayOfWeek.Monday: return DaysInWeek.Monday;
                case DayOfWeek.Tuesday: return DaysInWeek.Tuesday;
                case DayOfWeek.Wednesday: return DaysInWeek.Wednesday;
                case DayOfWeek.Thursday: return DaysInWeek.Thursday;
                case DayOfWeek.Friday: return DaysInWeek.Friday;

            }
            return value;
        }

        /// <summary>
        ///این عمل در WeekDayNameLocalizer پشتیبانی نمی شود
        /// </summary>
        /// <exception cref="NotSupportedException"  />
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
