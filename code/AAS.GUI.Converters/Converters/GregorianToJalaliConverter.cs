using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace AAS.GUI.Converters
{
    /// <summary>
    /// یک Converter برای تبدیل تاریخ میلادی به جلالی
    /// </summary>
    public class GregorianToJalaliConverter : MarkupExtension, IValueConverter
    {
        private PersianCalendar m_persianCalender;

        /// <summary>
        /// متد سازنده
        /// </summary>
        public GregorianToJalaliConverter()
        {
            m_persianCalender = new PersianCalendar();

        }

        /// <summary>
        /// تبدیل تاریح میلادی به جلالی
        /// </summary>
        /// <param name="value">تاریخ میلادی</param>
        /// <param name="targetType">نوع مقصد</param>
        /// <param name="parameter">این پارامتر در GregorianToJalaliConverter استفاده نمی شود</param>
        /// <param name="culture">مقدار اختصاص داده شده در CovnerterCulture</param>
        /// <returns>تاریج جلالی متناظر</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Thread.CurrentThread.CurrentUICulture = culture;
            DateTime gregorianDateTime = (DateTime)value;
            
            if (gregorianDateTime == DateTime.MinValue)
                gregorianDateTime = DateTime.Now;            
            
            return string.Format("{0}-{1}-{2}", m_persianCalender.GetYear(gregorianDateTime),
                                m_persianCalender.GetMonth(gregorianDateTime),
                                m_persianCalender.GetDayOfMonth(gregorianDateTime));
            
        }

        /// <summary>
        /// تبدیل تاریخ جلالی به میلادی
        /// </summary>
        /// <param name="value">تاریخ جلالی</param>
        /// <param name="targetType">نوع مقصد</param>
        /// <param name="parameter">این پارامتر در GregorianToJalaliConverter استفاده نمی شود</param>
        /// <param name="culture">مقدار اختصاص داده شده در CovnerterCulture</param>
        /// <returns>تاریح میلادی متناظر</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime jalaliDateTime ;
            if (value != null & value is DateTime)
            {


                Thread.CurrentThread.CurrentUICulture = culture;
                jalaliDateTime = (DateTime)value;

            }
            else
            {
                jalaliDateTime = DateTime.Now;
            }
            return m_persianCalender.ToDateTime(jalaliDateTime.Year, jalaliDateTime.Month, jalaliDateTime.Day, jalaliDateTime.Hour, jalaliDateTime.Minute, jalaliDateTime.Second, jalaliDateTime.Millisecond);

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
