using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace AAS.Entities
{
    /// <summary>
    /// وضعیت حضور
    /// </summary>
    public enum AttendanceStatus
    {
        /// <summary>
        /// حاضظر
        /// </summary>
        Present,

        /// <summary>
        /// غایب
        /// </summary>
        Absent

    }
    /// <summary>
    /// زمان حضور
    /// </summary>
    [DataContract]
    public class AttendanceTime : IEntity
    {
        private Guid m_ID;
        /// <summary>
        /// شناسه داخلی قابل استفاده در پایگاه داده
        /// </summary>
        [DataMember]
        public virtual Guid ID
        {
            get
            {
                return m_ID;
            }
            set
            {
                m_ID = value;
            }
        }

        private Employee m_employee;
        /// <summary>
        ///کارمند
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

        private DateTime m_entryTime;
        /// <summary>
        /// زمان ورود
        /// </summary>
        [DataMember]
        public virtual DateTime EntryTime
        {
            set
            {
                m_entryTime = value;
            }
            get
            {
                return m_entryTime;
            }
        }

        private DateTime m_exitTime;
        /// <summary>
        /// زمان خروج
        /// </summary>
        [DataMember]
        public virtual DateTime ExitTime
        {
            set
            {
                m_exitTime = value;
            }
            get
            {
                return m_exitTime;
            }
        }


        /// <summary>
        /// مقدار HashCode نمونه جاری
        /// </summary>
        /// <returns>مقدار HashCode</returns>
        public override int GetHashCode()
        {
            Func<object, int> getHashCode = (obj) => obj != null ? obj.GetHashCode() : 0;
            int hashcode = 0;

            const int num = 23;


            hashcode *= num + getHashCode(this.Employee);
            hashcode *= num + getHashCode(this.EntryTime);
            hashcode *= num + getHashCode(this.ExitTime);

            return hashcode;
        }

        /// <summary>
        /// برسی یکسان بودن نمونه جاری با نمونه ورودی
        /// </summary>
        /// <param name="obj">نمونه وردی</param>
        /// <returns>نتیجه برسی</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is AttendanceTime))
                return false;

            AttendanceTime A = this;
            AttendanceTime B = (AttendanceTime)obj;

            return
                A.Employee == B.Employee &&
                A.EntryTime == B.EntryTime &&
                A.ExitTime == B.ExitTime;
        }

        /// <summary>
        /// عملگر ==
        /// </summary>
        /// <param name="A">نمونه اول</param>
        /// <param name="B">نمونه دوم</param>
        /// <returns>نتیجه برسی</returns>
        public static bool operator ==(AttendanceTime A, AttendanceTime B)
        {
            if (object.ReferenceEquals(A, B))
                return true;

            if ((object)A == null || (object)B == null)
                return false;


            return A.Equals(B);
        }

        /// <summary>
        /// عملگر !=
        /// </summary>
        /// <param name="A">نمونه اول</param>
        /// <param name="B">نمونه دوم</param>
        /// <returns>نتیجه برسی</returns>
        public static bool operator !=(AttendanceTime A, AttendanceTime B)
        {
            return !(A == B);
        }

        /// <summary>
        /// نمایش رشته ای AttendanceTime
        /// </summary>
        /// <returns>رشته متناظر با IRNationalID</returns>
        public override string ToString()
        {
            return string.Format("{0} ({1} - {2}, {3})", Employee.ToString(), EntryTime.ToShortTimeString(), ExitTime.ToShortTimeString());

        }

        /// <summary>
        /// برسی حضور در روز انتخاب شده
        /// </summary>
        /// <param name="attendanceTime">ساعت حضور</param>
        /// <param name="dateTime">تاریخ مورد نظر</param>
        /// <returns>نتیجه برسی</returns>
        public static bool IsPresent(AttendanceTime attendanceTime, DateTime dateTime)
        {
            return
                (
                    (attendanceTime.EntryTime.Year == dateTime.Year &&
                    attendanceTime.EntryTime.Month == dateTime.Month &&
                    attendanceTime.EntryTime.Day == dateTime.Day)
                ||
                    (attendanceTime.ExitTime.Year == dateTime.Year &&
                    attendanceTime.ExitTime.Month == dateTime.Month &&
                    attendanceTime.ExitTime.Day == dateTime.Day)
                );

        }
       
   

    }


}
