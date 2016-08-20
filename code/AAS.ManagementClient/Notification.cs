using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AAS.ManagementClient
{
    /// <summary>
    /// نوع اعلام
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// اطلاعات
        /// </summary>
        Info,
        /// <summary>
        /// هشدار
        /// </summary>
        Warning,
        /// <summary>
        /// خطا
        /// </summary>
        Error
    }
    /// <summary>
    /// اعلان
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// پیغام
        /// </summary>
        public string Message { set; get; }

        /// <summary>
        /// نوع اعلان
        /// </summary>
        public NotificationType Type { set; get; }

        /// <summary>
        /// رنگ مربوط به نوع اعلان
        /// </summary>
        public SolidColorBrush TypeColor
        {
            get
            {
                switch (this.Type)
                {
                    case NotificationType.Info:     return Brushes.White;
                    case NotificationType.Warning:  return Brushes.Orange;
                    case NotificationType.Error:    return Brushes.Red;
                    default:                        return Brushes.Transparent;
                }
                
            }
        }
    }
}
