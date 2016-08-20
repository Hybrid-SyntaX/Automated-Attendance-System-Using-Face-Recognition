using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAS.Entities.Exceptions;
using AAS.Util;
using AAS.Entities;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using System.Drawing;
using NHibernate.Cfg.MappingSchema;
using AAS.Persistence;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.IO.Compression;
using System.Drawing.Imaging;
using PictureRepository;
using System.IO;
using System.Globalization;

namespace AAS.Service
{
    /// <summary>
    /// سرویس خودکار حضور و غیاب
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class AutomatedAttendanceSystem : IAutomatedAttendanceSystem
    {

        private void INSERT_SAMPLE_DATA(bool insert = true, bool truncate_old_data = false)
        {
            if (!insert) return;

            if (truncate_old_data)
                PersistenceObjectRepository<AttendanceTime>.Truncate();

            if (false)
                foreach (Employee employee in RetrieveEmployees())
                {

                    employee.WorkSchedule.DefineRange(DayOfWeek.Saturday, 9, 17, WorkSchedule.State.Work);
                    employee.WorkSchedule.DefineRange(DayOfWeek.Sunday, 9, 17, WorkSchedule.State.Work);
                    employee.WorkSchedule.DefineRange(DayOfWeek.Monday, 9, 17, WorkSchedule.State.Work);
                    employee.WorkSchedule.DefineRange(DayOfWeek.Tuesday, 9, 17, WorkSchedule.State.Work);
                    employee.WorkSchedule.DefineRange(DayOfWeek.Wednesday, 9, 17, WorkSchedule.State.Work);
                    employee.WorkSchedule.DefineRange(DayOfWeek.Thursday, 9, 17, WorkSchedule.State.Work);
                    PersistenceObjectRepository<Employee>.Update(employee);
                }

            List<AttendanceTime> attendanceTimeSampleData = new List<AttendanceTime>();
            int year = 2014;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day - 5;
            DayOfWeek dayofWeek = (new DateTime(year, month, day)).DayOfWeek;

            attendanceTimeSampleData.Add(new AttendanceTime()
            {
                Employee = RetrieveEmployees()[4], //Binda Binder
                //AttendanceHours = new WorkSchedule(),
                EntryTime = new DateTime(year, month, day, 09, 0, 0, 0),
                ExitTime = new DateTime(year, month, day, 21, 0, 0, 0)
            });


            attendanceTimeSampleData.Add(new AttendanceTime()
            {
                Employee = RetrieveEmployees()[2], //محمد طالبی
                //   AttendanceHours = new WorkSchedule(),
                EntryTime = new DateTime(year, month, day, 09, 0, 0, 0),
                ExitTime = new DateTime(year, month, day, 17, 0, 0, 0)
            });


            attendanceTimeSampleData.Add(new AttendanceTime()
            {
                Employee = RetrieveEmployees()[0], //Abbas Allahyari
                EntryTime = new DateTime(year, month, day, 18, 0, 0),
                ExitTime = new DateTime(year, month, day, 23, 0, 0),
                //    AttendanceHours = new WorkSchedule(),
            });


            foreach (AttendanceTime at in attendanceTimeSampleData)
                PersistenceObjectRepository<AttendanceTime>.Create(at);

            List<ContactInformation> contactInformationSampleData = new List<ContactInformation>()
            {
                new ContactInformation()
                {
                    Label = "Home",
                    PhoneNumber = "02199115532",
                    CellphoneNumber = "09126512321",
                    Email = "aaa@bbb.com",
                    Address = "No 3, Valiasr, Tehran",
                    PostalCode = "1434567891",
                    Employee=RetrieveEmployees()[0]
                },
                new ContactInformation()
                {
                    Label = "Grandpa",
                    PhoneNumber = "02188115532",
                    CellphoneNumber = "09326512321",
                    Email = "ccc@adad.com",
                    Address = "No 5, Ponak, Tehran",
                    PostalCode = "1434567893",
                    Employee=RetrieveEmployees()[0]
                }
            };
#if false
            RetrieveEmployees()[0].ContactInformations.Add(contactInformationSampleData[0]);
            RetrieveEmployees()[0].ContactInformations.Add(contactInformationSampleData[1]);
            PersistenceObjectRepository<Employee>.Update(RetrieveEmployees()[0]);
#endif


        }

        /// <summary>
        /// کل سیتسم را ریست می کند، استفاده در موارد تست
        /// </summary>
        private void reset()
        {

            m_attendedEmployees = new List<Employee>();
            PersistenceObjectRepository<ContactInformation>.Truncate();
            PersistenceObjectRepository<AttendanceTime>.Truncate();
            PersistenceObjectRepository<Employee>.Truncate();
            PersistenceObjectRepository<Log>.Truncate();
            m_instace = new AutomatedAttendanceSystem();



        }

        private List<Employee> m_attendedEmployees;

        private PictureRepository.PictureRepository m_profilePictureRepository = new PictureRepository.PictureRepository(@"profilepictures.zip");
        private PictureRepository.PictureRepository m_logPictureRepository = new PictureRepository.PictureRepository(@"logpictures.zip");


        private static AutomatedAttendanceSystem m_instace;

        /// <summary>
        /// ایجاد یک نمونه
        /// </summary>
        /// <returns>یک نمونه از <c>AutomatedAttendanceSystem</c></returns>
        public static AutomatedAttendanceSystem CreateInstance()
        {
            if (m_instace == null)
                m_instace = new AutomatedAttendanceSystem();
            return m_instace;
        }

        /// <summary>
        /// متد سازنده
        /// </summary>
        private AutomatedAttendanceSystem()
        {
            m_attendedEmployees = new List<Employee>();
            NHibernateHelper.InitializeDatabase("AASDatabase",
            new List<Type>() 
                { 
                    typeof(Persistence.Mappings.ContactInformationMap),
                    typeof(Persistence.Mappings.EmployeeMap) ,
                    typeof(Persistence.Mappings.AttendanceTimeMap),
                    typeof(Persistence.Mappings.LogMap)
                });

            // INSERT_SAMPLE_DATA(false, false);


            if (!File.Exists(@"profilepictures.zip"))
            {
                using (ZipArchive archive = ZipFile.Open(@"profilepictures.zip", ZipArchiveMode.Create)) ;
            }
            if (!File.Exists(@"logpictures.zip"))
            {
                using (ZipArchive archive = ZipFile.Open(@"logpictures.zip", ZipArchiveMode.Create)) ;
            }
        }

        /// <summary>
        /// راه اندازی سیستم خودکار حضور و غیاب
        /// </summary>
        public void Initialize()
        {
            NHibernateHelper.InitializeDatabase("AASDatabase",
         new List<Type>() 
                        { 
                            typeof(Persistence.Mappings.ContactInformationMap),
                            typeof(Persistence.Mappings.EmployeeMap),
                            typeof(Persistence.Mappings.AttendanceTimeMap)
                        });
        }

        #region Employee CRUD

        /// <summary>
        /// ایجاد شماره کارمندی جدید
        /// </summary>
        /// <returns>شماره کارمندی</returns>
        public EmployeeID NewEmployeeID()
        {
            return EmployeeID.NewEmployeeID(RetrieveEmployees());
        }

        /// <summary>
        ///بازیابی تمام کارمندان
        /// </summary>
        /// <returns>لیستی از تمام کارمندان</returns>
        public List<Employee> RetrieveEmployees()
        {
            var employeeList = PersistenceObjectRepository<Employee>.RetrieveAll();
            foreach (Employee e in employeeList)
                e.ProfilePicture = m_profilePictureRepository.Read(e.ID.ToString());

            return employeeList;
        }

        /// <summary>
        /// بازیابی کارمند با شناسه داخلی
        /// </summary>
        /// <param name="id">شناسه داخلی</param>
        /// <returns>کارمند</returns>
        public Employee RetrieveEmployee(Guid id)
        {
            var employee = PersistenceObjectRepository<Employee>.Retrieve(id);
            if (employee != null)
                employee.ProfilePicture = m_profilePictureRepository.Read(employee.ID.ToString());
            return employee;
        }

        /// <summary>
        /// بازیابی کارمند با شماره کارمندی
        /// </summary>
        /// <param name="employeeId">شماره کارمندی</param>
        /// <returns>کارمند</returns>
        public Employee RetrieveEmployee(EmployeeID employeeId)
        {
            Employee employee = RetrieveEmployees().FirstOrDefault((emp) => emp.EmployeeID == employeeId);
            if (employee != null)
                employee.ProfilePicture = m_profilePictureRepository.Read(employee.ID.ToString());
            return employee;
        }

        /// <summary>
        /// ثبت کارمند جدید
        /// </summary>
        /// <param name="employee">کارمند</param>
        /// <returns>کارمند ثبت شده</returns>
        public Employee CreateEmployee(Employee employee)
        {

            PersistenceObjectRepository<Employee>.Create(employee);

            m_profilePictureRepository.Save(employee.ID.ToString(), employee.ProfilePicture);
            return employee;
        }

        /// <summary>
        /// حذف کارمند
        /// </summary>
        /// <param name="employee">کازمند</param>
        public void DeleteEmployee(Employee employee)
        {
            PersistenceObjectRepository<Employee>.Delete(employee);
            m_profilePictureRepository.Delete(employee.ID.ToString());

        }

        /// <summary>
        /// بروز رسانی اطلاعات کارمند
        /// </summary>
        /// <param name="employee">کارمند</param>
        /// <returns>کارمند با اطلاعات بروز شده</returns>
        public Employee UpdateEmployee(Employee employee)
        {
            Employee updatedEmployee = PersistenceObjectRepository<Employee>.Update(employee);
            m_profilePictureRepository.Save(updatedEmployee.ID.ToString(), updatedEmployee.ProfilePicture);
            return updatedEmployee;
        }

        /// <summary>
        /// ثبت کامند جدید یا بروز رسانی کارمند موجود
        /// </summary>
        /// <param name="employee">کارمند</param>
        /// <returns>کارمند جدید، یا کارمند با اطلاعات بروز شده</returns>
        public Employee CreateOrUpdateEmployee(Employee employee)
        {
            return PersistenceObjectRepository<Employee>.CreateOrUpdate(employee);
        }

        #endregion

        #region ContactInformation CRUD

        /// <summary>
        /// ثبت اطلاعات تماس جدید
        /// </summary>
        /// <param name="contactInformation">اطلاعات تماس</param>
        /// <returns>ا</returns>
        public ContactInformation CreateContactInformation(ContactInformation contactInformation)
        {
            return PersistenceObjectRepository<ContactInformation>.Create(contactInformation);
        }


        #endregion


        #region AttendanceTime CRUD


        /// <summary>
        ///ثبت ساعت ورودی
        /// </summary>
        /// <param name="employee">کارمند</param>
        /// <returns>زمان حضور</returns>
        public AttendanceTime RegisterEmployeeEntryTime(Employee employee)
        {
            ///[Registering entry time]
            AttendanceTime attendanceTime = new AttendanceTime()
            {
                Employee = employee,
                EntryTime = DateTime.Now,
            };

            return PersistenceObjectRepository<AttendanceTime>.Create(attendanceTime);
            ///[Registering entry time]
        }

        /// <summary>
        /// برگرداندن لیست تمامی ساعات حضور
        /// </summary>
        /// <returns>لیستی از تمام ساعات حضور</returns>
        public List<AttendanceTime> RetrieveAttendanceTimes()
        {
            return PersistenceObjectRepository<AttendanceTime>.RetrieveAll();
        }

        /// <summary>
        /// ثبت ساعت خروج
        /// </summary>
        /// <param name="employee">کارمند</param>
        /// <returns>زمان خروج</returns>
        public AttendanceTime RegisterEmployeeExitTime(Employee employee)
        {
            if (employee == null)
                return null;
            ///[Registering exit time]
            AttendanceTime attendanceTime = new AttendanceTime();

            attendanceTime = RetrieveAttendanceTimes().Where((at) => at.Employee == employee).Last();
            attendanceTime.Employee.ProfilePicture = m_profilePictureRepository.Read(attendanceTime.Employee.ID.ToString());
            attendanceTime.ExitTime = DateTime.Now;

            return PersistenceObjectRepository<AttendanceTime>.Update(attendanceTime);
            ///[Registering exit time]
        }

        /// <summary>
        /// ثبت ورود/خروج
        /// این متد به طور خورکار ورود و خروج را ثبت می کند
        /// </summary>
        /// <param name="employee">کارمند</param>
        /// <returns>زمان حضور</returns>
        public AttendanceTime RegisterAttendance(Employee employee)
        {
            ///[Registering attendance]
            AttendanceTime attendanceTime = null;
            if (!m_attendedEmployees.Contains(employee) || m_attendedEmployees.Count == 0)
            {
                attendanceTime = RegisterEmployeeEntryTime(employee);
                m_attendedEmployees.Add(employee);
            }
            else
            {
                attendanceTime = RegisterEmployeeExitTime(employee);
                m_attendedEmployees.Remove(employee);
            }

            return attendanceTime;
            ///[Registering attendance]
        }
        #endregion


        #region Logs
        /// <summary>
        /// بازیابی خطا ها
        /// </summary>
        /// <returns>لیستی از تمام خطا ها</returns>
        public List<Log> RetrieveLogs()
        {
            var logList = PersistenceObjectRepository<Log>.RetrieveAll();
            foreach (Log log in logList)
                log.Picture = m_logPictureRepository.Read(log.ID.ToString());
            return logList;
        }

        /// <summary>
        /// ثبت گزارش خطای جدید
        /// </summary>
        /// <param name="log">گزارش خطا</param>
        /// <returns>گزارش خطای ثبت شده</returns>
        public Log CreateLog(Log log)
        {
            Log l_log = PersistenceObjectRepository<Log>.Create(log);

            m_logPictureRepository.Save(l_log.ID.ToString(), l_log.Picture);

            return log;
        }
        #endregion

        /// <summary>
        /// ثبت لیستی از خطا ها
        /// </summary>
        /// <param name="logs">لیستی از خطا ها</param>
        public void CreateLogs(IList<Log> logs)
        {
            PersistenceObjectRepository<Log>.Create(logs);
            foreach (Log log in logs)
                m_logPictureRepository.Save(log.ID.ToString(), log.Picture);
        }
    }
}