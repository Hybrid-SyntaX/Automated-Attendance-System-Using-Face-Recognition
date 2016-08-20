using System;
using System.Collections.Generic;
using System.Drawing;
namespace AAS.Entities.Interfaces
{
    /// <summary>
    /// انترفیس IEmployee
    /// </summary>
    public interface IEmployee
    {
        /// <summary>
        /// اطلاعات تماس
        /// </summary>
        IList<ContactInformation> ContactInformations { get; set; }
        
        /// <summary>
        /// ساعات حضور
        /// </summary>
        IList<AttendanceTime> AttendanceTimes { set; get; }
        
        /// <summary>
        /// تاریخ تولد
        /// </summary>
        DateTime DateOfBirth { get; set; }
        
        /// <summary>
        /// تاریخ استخدام
        /// </summary>
        DateTime DateOfEmployement { get; set; }

        /// <summary>
        /// شماره کارمندی
        /// </summary>
        EmployeeID EmployeeID { get; set; }

        /// <summary>
        /// نام 
        /// </summary>
        string FirstName { get; set; }

        /// <summary>
        /// نام کامل
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// جنسیت
        /// </summary>
        Gender Gender { get; set; }

        /// <summary>
        /// شماره داخلی قابل استفاده در پایگاه داده
        /// </summary>
        Guid ID { get; set; }

        /// <summary>
        /// شناسه داخلی
        /// </summary>
        string IdentityNumber { get; set; }

        /// <summary>
        /// نام خانوادگی
        /// </summary>
        string LastName { get; set; }

        /// <summary>
        /// کد ملی
        /// </summary>
        IRNationalID NationalID { get; set; }

        /// <summary>
        /// عکس پرسنلی
        /// </summary>
        Bitmap ProfilePicture { get; set; }

        /// <summary>
        /// ساعات کاری
        /// </summary>
        WorkSchedule WorkSchedule { get; set; }
    }
}
