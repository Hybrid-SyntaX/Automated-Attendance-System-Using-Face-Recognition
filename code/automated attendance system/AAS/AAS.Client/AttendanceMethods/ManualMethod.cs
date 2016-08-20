using AAS.Entities;
using AAS.Proxy;
using AAS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAS.Client.AttendanceMethods
{
    /// <summary>
    /// ورود کد کارمندی به طور دستی
    /// </summary>
    public class ManualMethod : IAttendanceMethod
    {
        private IAutomatedAttendanceSystem AutomatedAttendanceSystem;

        /// <summary>
        /// متد سازنده
        /// </summary>
        public ManualMethod()
        {
            AutomatedAttendanceSystem = new AutomatedAttendanceSystemProxy();
        }


        private object input;
        /// <summary>
        /// زشته وزودی
        /// </summary>
        public object Input
        {
            get
            {
                return input;
            }
            set
            {
                input = value;
            }
        }
        /// <summary>
        /// برسی صحت ورودی
        /// </summary>
        /// <returns>نتیجه برسی</returns>
        public bool Check()
        {
            return Input != null && (EmployeeID)Input != EmployeeID.Empty;
        }

        /// <summary>
        /// برسی وجود کد کارمندی وارد شده، و بر گرداندن شناسه داخلی
        /// </summary>
        /// <returns>شناسه داخلی یا مقدار Guid.Empty</returns>
        public object Run()
        {
            ///[Checking for EmployeeID]
            EmployeeID employeeId = (EmployeeID)Input;
            if (employeeId == null || employeeId == EmployeeID.Empty)
                return Guid.Empty;

            var e = AutomatedAttendanceSystem.RetrieveEmployee(employeeId);
            if (e != null && e.ID != Guid.Empty)
                return e.ID;
            else return Guid.Empty;
            ///[Checking for EmployeeID]
        }
    }
}
