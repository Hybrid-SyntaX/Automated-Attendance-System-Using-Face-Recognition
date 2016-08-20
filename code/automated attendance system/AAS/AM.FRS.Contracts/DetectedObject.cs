using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
namespace AM.FRS.Contracts
{
    [DataContract]
    /// یک شی شناسایی شده
    public class DetectedObject
    {
        /// <summary>
        /// متد سازنده بدون پارامتر <c>DetectedObject</c>
        /// </summary>
        public DetectedObject()
        {

        }

        /// <summary>
        /// متد سازنده دو پارامتر <c>DetectedObject</c>
        /// </summary>
        /// <param name="bitmap">تصویر تشخیص داده شده</param>
        /// <param name="region">نواحی که در آن تصویر تشخیص شده قرار دارد</param>
        public DetectedObject(Bitmap bitmap,Rectangle region)
        {
            m_bitmap = bitmap;
            m_reigion = region;
        }

        [DataMember]
        private Bitmap m_bitmap;

        /// <summary>
        /// تصویر تشخیص داده شده
        /// </summary>
        public Bitmap Bitmap 
        { 
            get
            {
                return m_bitmap;
            }
        }

        [DataMember]
        private Rectangle m_reigion;
        /// <summary>
        /// نواحی که در آن تصویر تشخیص شده قرار دارد
        /// </summary>
        public Rectangle Region 
        { 
            get
            {
                return m_reigion;
            }
        }
    }
}
