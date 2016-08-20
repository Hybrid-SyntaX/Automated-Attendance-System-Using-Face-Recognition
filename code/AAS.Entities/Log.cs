using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AAS.Entities
{
    /// <summary>
    /// روش اعلام حضور
    /// </summary>
    public enum AttendanceMethod
    {
        /// <summary>
        /// تشخیص چهره
        /// </summary>
        FaceRecognition,
        /// <summary>
        /// کد QR
        /// </summary>
        QRCode,
        /// <summary>
        /// ورود دستی شماره کارمندی
        /// </summary>
        ManualEmployeeIDEntry
    }
    /// <summary>
    /// نتیجه اعلام حضور
    /// </summary>
    public enum AttendanceMethodResult
    {
        /// <summary>
        /// شکست
        /// </summary>
        Failure,
        /// <summary>
        /// موفقیت
        /// </summary>
        Success,
        /// <summary>
        /// رد شده
        /// </summary>
        Reject,
        /// <summary>
        /// قبول شده
        /// </summary>
        Accept
    }

    /// <summary>
    /// گزارش خطا
    /// </summary>
    [DataContract]
    public class Log : IEntity
    {

        private Guid m_id;
        /// <summary>
        /// شناسه داخلی
        /// </summary>
        [DataMember]
        public virtual Guid ID
        {
            set
            {
                m_id = value;
            }
            get
            {
                return m_id;
            }
        }

        private DateTime m_EventTime;
        /// <summary>
        /// زمان وقوع
        /// </summary>
        [DataMember]
        public virtual DateTime EventTime
        {
            set
            {
                m_EventTime = value;
            }
            get
            {
                return m_EventTime;
            }
        }

        private Employee m_employee;
        /// <summary>
        /// کارمند تشخیص داده شده
        /// </summary>
        [DataMember]
        public virtual Employee Employee
        {
            set
            {
                m_employee = value;
            }
            get
            {
                return m_employee;
            }
        }

        private Bitmap m_picture;
        /// <summary>
        /// تصویر
        /// </summary>
        [DataMember]
        public virtual Bitmap Picture
        {
            set
            {
                m_picture = value;
            }
            get
            {
                return m_picture;
            }
        }

        private AttendanceMethod m_attendanceMethod;
        /// <summary>
        /// روش اعلام عضور
        /// </summary>
        [DataMember]
        public virtual AttendanceMethod AttendanceMethod
        {
            set
            {
                m_attendanceMethod = value;
            }
            get
            {
                return m_attendanceMethod;
            }
        }


        private AttendanceMethodResult m_attendanceMethodResult;
        /// <summary>
        /// نتیجه اعلام حضور
        /// </summary>
        [DataMember]
        public virtual AttendanceMethodResult AttendanceMethodResult
        {
            set
            {
                m_attendanceMethodResult = value;
            }
            get
            {
                return m_attendanceMethodResult;
            }
        }

        private string m_enteredEmployeeID;
        /// <summary>
        /// شماره کارمندی وارد شده
        /// </summary>
        [DataMember]
        public virtual string EnteredEmployeeID
        {
            set
            {
                m_enteredEmployeeID = value;
            }
            get
            {
                return m_enteredEmployeeID;
            }
        }

      
    }
}
