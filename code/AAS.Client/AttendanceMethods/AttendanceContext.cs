using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAS.Client.AttendanceMethods
{
    /// <summary>
    /// ارگمان های رویداد های مربوط به روش های اعلام حضور
    /// </summary>
    public class AttendanceMethodEventArgs : EventArgs
    {
        /// <summary>
        /// متد سازنده
        /// </summary>
        /// <param name="input">مقدار ورودی برای برسی</param>
        /// <param name="runReturnValue">مقدار خروجی</param>
        public AttendanceMethodEventArgs(object input, object runReturnValue)
        {
            m_input = input;
            m_runReturnValue = runReturnValue;
        }
        private object m_runReturnValue;

        /// <summary>
        /// مقدار خروجی پس از اجرای روش
        /// </summary>
        public object RunReturnValue
        {
            get
            {
                return m_runReturnValue;
            }
        }
        private object m_input;
        /// <summary>
        /// مقدار ورودی
        /// </summary>
        public object Input
        {
            get
            {
                return m_input;
            }
        }
    }

    /// <summary>
    /// این کلاس مسئول اجرای این روش ها می باشد.
    /// </summary>
    public class AttendanceContext
    {

        private IAttendanceMethod m_attendanceMethod;

        /// <summary>
        /// متد سازنده
        /// </summary>
        /// <param name="attendanceMethod">روش اعلام حضور</param>
        /// <param name="input">ورودی روش</param>
        public AttendanceContext(IAttendanceMethod attendanceMethod, object input = null)
        {
            m_attendanceMethod = attendanceMethod;
            if (input != null)
                m_attendanceMethod.Input = input;
        }

        /// <summary>
        /// به روز کرده مقدار ورودی
        /// </summary>
        /// <param name="input"></param>
        /// <returns>نمونه جاری AttendanceContext</returns>
        public AttendanceContext UpdateInput(object input)
        {
            m_attendanceMethod.Input = input;
            return this;
        }

        /// <summary>
        /// اجرای روش
        /// </summary>
        /// <returns>شناسه داخلی</returns>
        public Guid Run()
        {
            ///[Running method]
            Guid id = Guid.Empty;
            if (m_attendanceMethod.Check())
            {
                if (CheckSuccess != null)
                    CheckSuccess(m_attendanceMethod, new AttendanceMethodEventArgs(m_attendanceMethod.Input, id));
                id = (Guid)m_attendanceMethod.Run();

                if (id != Guid.Empty)
                {
                    if (Success != null)
                        Success(m_attendanceMethod, new AttendanceMethodEventArgs(m_attendanceMethod.Input, id));
                }
                else
                    if (Failure != null)
                        Failure(m_attendanceMethod, new AttendanceMethodEventArgs(m_attendanceMethod.Input, id));

            }
            else
            {
                if (CheckFailure != null)
                    CheckFailure(m_attendanceMethod, new AttendanceMethodEventArgs(m_attendanceMethod.Input, id));
            }
            return id;
            ///[Running method]
        }
        /// <summary>
        /// Run با موفقیت مواجه شد
        /// </summary>
        public event EventHandler<AttendanceMethodEventArgs> Success;

        /// <summary>
        /// Run با شکست مواجه شد 
        /// </summary>
        public event EventHandler<AttendanceMethodEventArgs> Failure;
        
        /// <summary>
        /// Check موفقیت آمیز بود
        /// </summary>
        public event EventHandler<AttendanceMethodEventArgs> CheckSuccess;

        /// <summary>
        /// Check با شکست مواجه شده
        /// </summary>
        public event EventHandler<AttendanceMethodEventArgs> CheckFailure;

    }
}
