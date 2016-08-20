using AAS.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace AAS.GUI.Converters
{
    /// <summary>
    /// تبدیل یک WorkSchedule به یک لیست Boolean.
    /// این Converter ثبت بلادرنگ ساعات کاری در روز در نمونه WorkSchedule استفاده می شود.
    /// </summary>
    public class WorkScheduleToBooleanListConverter : MarkupExtension, IValueConverter
    {
        private WorkSchedule m_workSchedule;
        private DayOfWeek[] m_daysOfWeek = new DayOfWeek[] { DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };

        /// <summary>
        /// تبدیل WorkSchedule به یک لیست از Boolean
        /// </summary>
        /// <param name="value">یک WorkSchedule</param>
        /// <param name="targetType">نوع مقصد</param>
        /// <param name="parameter">این پارامتر در WorkScheduleToBooleanListConverter استفاده نمی شود</param>
        /// <param name="culture">این پارامتر در WorkScheduleToBooleanListConverter استفاده نمی شود</param>
        /// <returns>یک لیست از Boolean متناظر با WorkSchedule</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ///[Converting WorkSchedule to boolean list]
            if (value is WorkSchedule)
            {

                ObservableCollection<ObservableCollection<object>> boolList = new ObservableCollection<ObservableCollection<object>>();
                if (m_workSchedule == null || ((WorkSchedule)value).Updated)
                    m_workSchedule = (WorkSchedule)value;



                int i = 0;
                foreach (DayOfWeek day in m_daysOfWeek)
                {
                    ObservableCollection<object> hours = new ObservableCollection<object>();
                    int j = 0;

                    foreach (bool hour in m_workSchedule[day])
                    {

                        ObjectValue objValue = new ObjectValue(hour);
                        objValue.Row = i;
                        objValue.Column = j;
                        objValue.PropertyChanged += objValue_PropertyChanged;
                        hours.Add(objValue);
                        j++;
                    }
                    i++;

                    boolList.Add(hours);
                }
                return boolList;
            }

            return value;
            ///[Converting WorkSchedule to boolean list]
        }
        private void objValue_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ///[ObjectValue PropertyChanged]
            ObjectValue objecetValue = sender as ObjectValue;

            m_workSchedule.Define(m_daysOfWeek[objecetValue.Row], objecetValue.Column,
                ((bool)objecetValue.Value) ? WorkSchedule.State.Work : WorkSchedule.State.Off);
            ///[ObjectValue PropertyChanged]
        }

        /// <summary>
        /// تبدیل یک لیست Boolean به WorkSchedule
        /// </summary>
        /// <param name="value">لیستی از Boolean</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">این پارامتر در WorkScheduleToBooleanListConverter استفاده نمی شود</param>
        /// <param name="culture">این پارامتر در WorkScheduleToBooleanListConverter استفاده نمی شود</param>
        /// <returns>WorkSchedule متاظر</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ///[Converting list of boolean to WorkSchedule]
            if (targetType == typeof(WorkSchedule))
                return m_workSchedule;
            else return value;
            ///[Converting list of boolean to WorkSchedule]
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
