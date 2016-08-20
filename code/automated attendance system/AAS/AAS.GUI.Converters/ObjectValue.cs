using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAS.GUI
{
    /// <summary>
    /// یک کلاس کمکی برای تبدیل متغیر ها به Property و نمایش مقدار آن در XAML
    /// این کلاس بخصوص برای نمایش ساعات کار استفاده می شود
    /// </summary>
    public class ObjectValue : INotifyPropertyChanged
    {
        /// <summary>
        /// متد سازنده
        /// </summary>
        public ObjectValue()
        {

        }

        /// <summary>
        /// متد سازنده
        /// </summary>
        /// <param name="value">نمونه ای که قرار است تبدیل به <c>ObjectValue</c> شود</param>
        public ObjectValue(object value)
        {
            this.Value = value;
        }

        /// <summary>
        /// سطر
        /// </summary>
        public int Row { set; get; }
        
        /// <summary>
        /// ستون
        /// </summary>
        public int Column { set; get; }
     
        private object m_value;
        /// <summary>
        /// نمونه
        /// </summary>
        /// <value>m_value</value>
        public object Value
        {
            set
            {
                m_value = value;
                NotifyPropertyChanged("Value");
            }
            get
            {
                return m_value;
            }
        }
        
        /// <summary>
        /// رویداد مربوط به تغییر Property
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// اطلاع رسانی از تغییر Property
        /// </summary>
        /// <param name="propertyName">نام Property</param>
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public override string ToString()
        {
            if (Value != null)
                return Value.ToString();
            else return string.Empty;
        }
    }


}
