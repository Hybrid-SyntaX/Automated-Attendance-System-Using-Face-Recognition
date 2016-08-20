using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using AAS.Entities;
using AAS.Service;
namespace AAS.Proxy
{
    /// <summary>
    /// پراکسی سرویس AutomatedAttendanceSystem
    /// </summary>
    public class AutomatedAttendanceSystemProxy : ClientBase<IAutomatedAttendanceSystem>, IAutomatedAttendanceSystem
    {

        #region Employee CRUD
        /// <summary>
        /// ایجاد شماره کارمندی جدید
        /// </summary>
        /// <returns>شماره کارمندی جدید</returns>
        public EmployeeID NewEmployeeID()
        {
            return Channel.NewEmployeeID();
        }

        /// <summary>
        /// ثبت کارمند جدید
        /// </summary>
        /// <param name="employee">کارمند</param>
        /// <returns>کارمند ثبت شده</returns>
        public Employee CreateEmployee(Employee employee)
        {
            return base.Channel.CreateEmployee(employee);
        }

        /// <summary>
        /// بازیابی تمام کارمندان
        /// </summary>
        /// <returns>لیستی از کارمندان</returns>
        public List<Employee> RetrieveEmployees()
        {
            return base.Channel.RetrieveEmployees();
        }

        /// <summary>
        /// بازیابی با استفاده از شماره کارمندی
        /// </summary>
        /// <param name="employeeId">شماره کارمندی</param>
        /// <returns>کارمند</returns>
        public Employee RetrieveEmployee(EmployeeID employeeId)
        {
            return Channel.RetrieveEmployee(employeeId);
        }

        /// <summary>
        /// بازیابی با استفاده از شناسه داخلی
        /// </summary>
        /// <param name="id">شناسه داخلی</param>
        /// <returns>کارمند</returns>
        public Employee RetrieveEmployee(Guid id)
        {
            return base.Channel.RetrieveEmployee(id);
        }

        /// <summary>
        /// حذف کارمند
        /// </summary>
        /// <param name="employee">کارمند</param>
        public void DeleteEmployee(Employee employee)
        {
            base.Channel.DeleteEmployee(employee);
        }

        /// <summary>
        /// بروز رسانی کارمند
        /// </summary>
        /// <param name="employee">کارمند</param>
        /// <returns>کارمند به روز رسانی شده </returns>
        public Employee UpdateEmployee(Employee employee)
        {
            return base.Channel.UpdateEmployee(employee);
        }


        #endregion


        #region Misc
        /// <summary>
        /// راه اندازی
        /// </summary>
        public void Initialize()
        {

            base.Channel.Initialize();
        } 
        #endregion

        #region Attendance Times

        /// <summary>
        /// بازیابی تمامی ساعات حضور
        /// </summary>
        /// <returns>لیستی از تمام ساعات حضور</returns>
        public List<AttendanceTime> RetrieveAttendanceTimes()
        {
            return Channel.RetrieveAttendanceTimes();

        }

        /// <summary>
        /// ثبت حضور
        /// </summary>
        /// <param name="employee">کارمند</param>
        /// <returns>ساعت حضور</returns>
        public AttendanceTime RegisterAttendance(Employee employee)
        {
            return Channel.RegisterAttendance(employee);
        } 
        #endregion

        #region Logs
        /// <summary>
        /// بازیابی خطا ها
        /// </summary>
        /// <returns>لیستی از تمام خطا ها</returns>
        public List<Log> RetrieveLogs()
        {
            return Channel.RetrieveLogs();
        }
        /// <summary>
        /// ثبت گزارش خطای جدید
        /// </summary>
        /// <param name="log">گزارش خطا</param>
        /// <returns>گزارش خطای ثبت شده</returns>
        public Log CreateLog(Log log)
        {
            return Channel.CreateLog(log);
        }

        /// <summary>
        /// ثبت لیستی از خطا ها
        /// </summary>
        /// <param name="logs">لیستی از خطا ها</param>
        public void CreateLogs(IList<Log> logs)
        {
            Channel.CreateLogs(logs);
        }
        #endregion





        
        /// <summary>
        /// ثبت یا بروزرسانی کارمند
        /// </summary>
        /// <param name="employee">کارمند</param>
        /// <returns>کارمند ثبت شده یا بروز رسانی شده</returns>
        public Employee CreateOrUpdateEmployee(Employee employee)
        {
            return base.Channel.CreateOrUpdateEmployee(employee);
        }
        /// <summary>
        /// ثبت اطلاعات تماس
        /// </summary>
        /// <param name="contactInformation">اطلاعات تماس</param>
        /// <returns>اطلاعات تماس ثبت شده</returns>
        public ContactInformation CreateContactInformation(ContactInformation contactInformation)
        {
            return this.Channel.CreateContactInformation(contactInformation);
        }

        /// <summary>
        /// ثبت ساعت ورود
        /// </summary>
        /// <param name="employee">کارمند</param>
        /// <returns>ساعت حضور</returns>
        public AttendanceTime RegisterEmployeeEntryTime(Employee employee)
        {
            return Channel.RegisterEmployeeEntryTime(employee);
        }
        /// <summary>
        /// ثبت ساعت خروج
        /// </summary>
        /// <param name="employee">کارمند</param>
        /// <returns>ساعت حضور</returns>
        public AttendanceTime RegisterEmployeeExitTime(Employee employee)
        {
            return Channel.RegisterEmployeeExitTime(employee);
        }


    }
}
