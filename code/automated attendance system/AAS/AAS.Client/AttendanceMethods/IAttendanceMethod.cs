using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAS.Client.AttendanceMethods
{
    /// <summary>
    /// IAttendanceMethod
    /// </summary>
    public interface IAttendanceMethod
    {
        /// <summary>
        /// ورودی
        /// </summary>
        object Input { set; get; }

        /// <summary>
        /// متد برای برسی آماده بودن
        /// </summary>
        /// <returns>نتیجه برسی</returns>
        bool Check();

        /// <summary>
        /// اجرای روش
        /// </summary>
        /// <returns>نتیجه</returns>
        object Run();
    }
}
