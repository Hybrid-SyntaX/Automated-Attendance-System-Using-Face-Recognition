using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using AAS.Entities;
using AAS.Service.Serialization;
using System.Drawing;
namespace AAS.Service
{
    /// <summary>
    /// اینترفیس IAutomatedAttendanceSystem
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IAutomatedAttendanceSystem
    {
        #region Misc
        /// <summary>
        /// راه اندازی
        /// </summary>
        [OperationContract]
        void Initialize();
        #endregion


        #region Employee CRUD
        /// <summary>
        /// ایجاد شماره کارمندی جدید
        /// </summary>
        /// <returns>شماره کارمندی جدید</returns>
        [OperationContract, UseNetDataContractSerializer]
        EmployeeID NewEmployeeID();

        /// <summary>
        /// ثبت کارمند جدید
        /// </summary>
        /// <param name="employee">کارمند</param>
        /// <returns>کارمند ثبت شده</returns>
        [OperationContract,UseNetDataContractSerializer]
        Employee CreateEmployee(Employee employee);

        /// <summary>
        /// ثبت یا بروزرسانی کارمند
        /// </summary>
        /// <param name="employee">کارمند</param>
        /// <returns>کارمند ثبت شده یا بروز رسانی شده</returns>
        [OperationContract, UseNetDataContractSerializer]
        Employee CreateOrUpdateEmployee(Employee employee);

        /// <summary>
        /// بروز رسانی کارمند
        /// </summary>
        /// <param name="employee">کارمند</param>
        /// <returns>کارمند به روز رسانی شده </returns>
        [OperationContract, UseNetDataContractSerializer]
        Employee UpdateEmployee(Employee employee);

        /// <summary>
        /// بازیابی تمام کارمندان
        /// </summary>
        /// <returns>لیستی از کارمندان</returns>
        [OperationContract, UseNetDataContractSerializer]
        List<Employee> RetrieveEmployees();

        /// <summary>
        /// بازیابی با استفاده از شماره کارمندی
        /// </summary>
        /// <param name="employeeId">شماره کارمندی</param>
        /// <returns>کارمند</returns>
        [OperationContract(Name = "RetrieveEmployeeByEmployeeID"), UseNetDataContractSerializer]
        Employee RetrieveEmployee(EmployeeID employeeId);
        
        /// <summary>
        /// بازیابی با استفاده از شناسه داخلی
        /// </summary>
        /// <param name="id">شناسه داخلی</param>
        /// <returns>کارمند</returns>
        [OperationContract(Name="RetrieveEmployeeByID"),UseNetDataContractSerializer]
        Employee RetrieveEmployee(Guid id);

        /// <summary>
        /// حذف کارمند
        /// </summary>
        /// <param name="employee">کارمند</param>
        [OperationContract, UseNetDataContractSerializer]
        void DeleteEmployee(Employee employee);


        #endregion

        #region ContactInformation CRUD
        /// <summary>
        /// ثبت اطلاعات تماس
        /// </summary>
        /// <param name="contactInformation">اطلاعات تماس</param>
        /// <returns>اطلاعات تماس ثبت شده</returns>
        [OperationContract]
        ContactInformation CreateContactInformation(ContactInformation contactInformation);
        #endregion

        #region AttendanceTime CRUD

        /// <summary>
        /// ثبت حضور
        /// </summary>
        /// <param name="employee">کارمند</param>
        /// <returns>ساعت حضور</returns>
        [OperationContract, UseNetDataContractSerializer]
        AttendanceTime RegisterAttendance(Employee employee);

        /// <summary>
        /// ثبت ساعت ورود
        /// </summary>
        /// <param name="employee">کارمند</param>
        /// <returns>ساعت حضور</returns>
        [OperationContract, UseNetDataContractSerializer]
        AttendanceTime RegisterEmployeeEntryTime(Employee employee);

        /// <summary>
        /// ثبت ساعت خروج
        /// </summary>
        /// <param name="employee">کارمند</param>
        /// <returns>ساعت حضور</returns>
        [OperationContract, UseNetDataContractSerializer]
        AttendanceTime RegisterEmployeeExitTime(Employee employee);

        /// <summary>
        /// بازیابی تمامی ساعات حضور
        /// </summary>
        /// <returns>لیستی از تمام ساعات حضور</returns>
        [OperationContract, UseNetDataContractSerializer]
        List<AttendanceTime> RetrieveAttendanceTimes();


        #endregion

        #region Logs
        /// <summary>
        /// ثبت گزارش خطا
        /// </summary>
        /// <param name="log">گزارش خطا</param>
        /// <returns>نمونه ثبت شده از گزارش خطا</returns>
        [OperationContract(Name = "CreateLogWithLog"), UseNetDataContractSerializer]
        Log CreateLog(Log log);

        /// <summary>
        /// ثبت لیستی از خطا ها
        /// </summary>
        /// <param name="logs">لیستی از خطا ها</param>
        [OperationContract(Name = "CreateLogWithLogList"), UseNetDataContractSerializer]
        void CreateLogs(IList<Log> logs);
        /// <summary>
        /// بازیابی تمامی گزارش های خطا
        /// </summary>
        /// <returns>لیستی از تمام گزارش های خطا</returns>
        [OperationContract, UseNetDataContractSerializer]
        List<Log> RetrieveLogs(); 
        #endregion



    }
}
