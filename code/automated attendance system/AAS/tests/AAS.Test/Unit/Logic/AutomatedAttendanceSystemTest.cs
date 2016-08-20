using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AAS.Entities.Exceptions;
using AAS.Entities;
using AAS.Persistence;
using AAS.Service;
namespace AAS.Entities.Test.Unit
{
    public class AutomatedAttendanceSystemTest : AssertionHelper
    {


        #region SetUp & TearDown & Constructor
        AutomatedAttendanceSystem AutomatedAttendanceSystem;
        static AutomatedAttendanceSystemTest()
        {

            //AutomatedAttendanceSystem.CreateEmployee(CommonTestCaseSourceProvider.newEmployeeSample());
            //AutomatedAttendanceSystem.CreateEmployee(CommonTestCaseSourceProvider.newEmployeeSample());
        }
        [SetUp]
        public void SetUp()
        {
            this.AutomatedAttendanceSystem = AutomatedAttendanceSystem.CreateInstance();
            AutomatedAttendanceSystem.Initialize();
            AutomatedAttendanceSystem.CreateEmployee(CommonTestCaseSourceProvider.newEmployeeSample());
            AutomatedAttendanceSystem.CreateEmployee(CommonTestCaseSourceProvider.newEmployeeSample());
        }
        [TearDown]
        public void TearDown()
        {
            //foreach(IEntity entity in garbage)
            //{
            //    PersistenceObjectRepository<IEntity>.Delete(entity);
            //}
            AAS.Test.Util.Mocker.CallPrivateMethod(typeof(AutomatedAttendanceSystem), this.AutomatedAttendanceSystem, "reset", null);
        }
        #endregion

        #region Employees

        [Test]
        public void Employees_WhenAccessed_CountIs2()
        {

            Expect(AutomatedAttendanceSystem.RetrieveEmployees().Count, Is.EqualTo(2));
        }
        #endregion

        #region RetrieveEmployees
        [Test]
        public void RetrieveEmployees_WhenCalled_CountIs2()
        {
            Expect(AutomatedAttendanceSystem.RetrieveEmployees().Count, Is.EqualTo(2));
        }
        #endregion

        #region RetrieveEmployee

        [Test]
        public void RetrieveEmployee_ProvidedWithID_ReturnsEmployeeInstance()
        {
            var returnValue = AutomatedAttendanceSystem.CreateEmployee(CommonTestCaseSourceProvider.newEmployeeSample());
            Guid retrievedEmployeeId = returnValue.ID;

            Expect(() => AutomatedAttendanceSystem.RetrieveEmployee(retrievedEmployeeId), Is.Not.Null);

        }
        #endregion

        #region DeleteEmployee
        [Test]
        public void DeleteEmployee_WhenCalled_EmployeeIsDeletedFromDatabase()
        {
            var returnValue = AutomatedAttendanceSystem.CreateEmployee(CommonTestCaseSourceProvider.newEmployeeSample());

            Expect(() => AutomatedAttendanceSystem.DeleteEmployee(returnValue), Throws.Nothing);

            Guid removedId = returnValue.ID;

            Expect(() => AutomatedAttendanceSystem.RetrieveEmployee(removedId), Is.Null);
        }

        #endregion

        #region CreateEmployee
        [Test]
        public void CreateEmployee_ProvidedWithEmployee_SavesInDatabase()
        {
            var returnValue = AutomatedAttendanceSystem.CreateEmployee(CommonTestCaseSourceProvider.newEmployeeSample());
            Expect(() => returnValue.ID, Is.Not.EqualTo(Guid.Empty));
        }
        [Test]
        public void CreateEmployee_WhenCalled_GeneratesANewEmployeeID()
        {

        }


        #endregion

        #region CreateContactInformation
        [Test]
        public void CreateContactInformation_ProvidedWithContactInformation_SavesInDatabase()
        {
            var contactInformation = AutomatedAttendanceSystem.CreateContactInformation(CommonTestCaseSourceProvider.newContactInformationSample());
            Expect(() => contactInformation.ID, Is.Not.EqualTo(Guid.Empty));
        }
        [Test]
        public void CreateContactInformation_ParameteredVersion_SavesInDatabase()
        {
            var contactInformation = AutomatedAttendanceSystem.CreateContactInformation(new ContactInformation()
            {
                Label = "Home",
                PhoneNumber = "02199115532",
                CellphoneNumber = "09126512321",
                Email = "aaa@bbb.com",
                Address = "No 3, Valiasr, Tehran",
                PostalCode = "1434567891",
            });
            Expect(() => contactInformation.ID, Is.Not.EqualTo(Guid.Empty));
        }
        #endregion

        #region RegisterEntryTime
        [Test]
        public void RetrieveAttendanceTimes_WhenCalled_ReturnsListOfAllAttendanceTimes()
        {
            ContactInformation contactInformation = null;
            Employee employee = null;
            Expect(() => { contactInformation = AutomatedAttendanceSystem.CreateContactInformation(CommonTestCaseSourceProvider.newContactInformationSample()); }, Throws.Nothing);
            Expect(() => { employee = AutomatedAttendanceSystem.CreateEmployee(CommonTestCaseSourceProvider.newEmployeeSample(contactInformation)); }, Throws.Nothing);
            Expect(() => { PersistenceObjectRepository<AttendanceTime>.Create(CommonTestCaseSourceProvider.newAttendanceTimeSample(employee)); }, Throws.Nothing);
            Expect(() => AutomatedAttendanceSystem.RetrieveAttendanceTimes().Count, Is.GreaterThan(0));
        }
        [Test]
        public void RegisterEmployeeEntryTime_WhenCalled_RegistersEntryTimeInDatabase()
        {
            ContactInformation contactInformation = null;
            Employee employee = null;
            Expect(() => { contactInformation = AutomatedAttendanceSystem.CreateContactInformation(CommonTestCaseSourceProvider.newContactInformationSample()); }, Throws.Nothing);
            Expect(() => { employee = AutomatedAttendanceSystem.CreateEmployee(CommonTestCaseSourceProvider.newEmployeeSample(contactInformation)); }, Throws.Nothing);

            DateTime entryTime = DateTime.Now;
            AttendanceTime attendanceTime = null;
            Expect(() => { attendanceTime = AutomatedAttendanceSystem.RegisterEmployeeEntryTime(employee); }, Throws.Nothing);
            Expect(() => attendanceTime.ID, Is.Not.EqualTo(Guid.Empty));
            Expect(() => AutomatedAttendanceSystem.RetrieveAttendanceTimes().Count, Is.EqualTo(1));

        }
        [Test]
        public void RegisterEmployeeExitTime_WhenCalled_CalculatesWorkHoursAndRegisterExitTimeInDatabase()
        {

            //Entry time is registered
            Employee employee = util_createSampleEmployeeAndContactInformation();
            AttendanceTime entryAttendanceTime = null;
            Expect(() => { entryAttendanceTime = AutomatedAttendanceSystem.RegisterEmployeeEntryTime(employee); }, Throws.Nothing);
            Expect(() => entryAttendanceTime.ID, Is.Not.EqualTo(Guid.Empty));

            //Mock Time?


            //Exit time is registered
            AttendanceTime exitAttendanceTime = null;
            Expect(() => { exitAttendanceTime = AutomatedAttendanceSystem.RegisterEmployeeExitTime(employee); }, Throws.Nothing);
            Expect(() => exitAttendanceTime.ID, Is.EqualTo(entryAttendanceTime.ID));
        //    Expect(() => exitAttendanceTime.AttendanceHours, Is.EqualTo(WorkSchedule.MinValue));
        }

        private Employee util_createSampleEmployeeAndContactInformation()
        {
            ContactInformation contactInformation = null;
            Employee employee = null;
            Expect(() => { contactInformation = AutomatedAttendanceSystem.CreateContactInformation(CommonTestCaseSourceProvider.newContactInformationSample()); }, Throws.Nothing);
            Expect(() => { employee = AutomatedAttendanceSystem.CreateEmployee(CommonTestCaseSourceProvider.newEmployeeSample(contactInformation)); }, Throws.Nothing);
            return employee;
        }
        #endregion


        #region RetrieveAttendaceTimes
        [Test]
        public void RetrieveAttendaceTimes_WhenCalled_ReturnsAllAttendanceTimes()
        {
            //Data
            var employee = CommonTestCaseSourceProvider.newEmployeeSample();
            var attendanceTime = CommonTestCaseSourceProvider.newAttendanceTimeSample(employee);

            //Tests
            Expect(() => employee = AutomatedAttendanceSystem.CreateEmployee(employee), Throws.Nothing);
            Expect(() => attendanceTime = PersistenceObjectRepository<AttendanceTime>.Create(attendanceTime), Throws.Nothing);

            Expect(() => AutomatedAttendanceSystem.RetrieveAttendanceTimes().Count, Is.EqualTo(1));
            Expect(() => AutomatedAttendanceSystem.RetrieveAttendanceTimes(), Is.TypeOf<List<AttendanceTime>>());
            Expect(() => AutomatedAttendanceSystem.RetrieveAttendanceTimes()[0], Is.TypeOf<AttendanceTime>());
            Expect(() => AutomatedAttendanceSystem.RetrieveAttendanceTimes()[0], Is.EqualTo(attendanceTime));
        }
        [Test]
        public void RetrieveAttendaceTimes_WhenCalledAfterRetrieveEmployees_Passes()
        {
            //Data
            var employee = CommonTestCaseSourceProvider.newEmployeeSample();
            var attendanceTime = CommonTestCaseSourceProvider.newAttendanceTimeSample(employee);

            //Tests
            Expect(() => employee = AutomatedAttendanceSystem.CreateEmployee(employee), Throws.Nothing);
            Expect(() => attendanceTime = PersistenceObjectRepository<AttendanceTime>.Create(attendanceTime), Throws.Nothing);
            
            Expect(() => AutomatedAttendanceSystem.RetrieveEmployees(),Throws.Nothing);
            Expect(() => AutomatedAttendanceSystem.RetrieveEmployees(), Is.Not.Empty);
            Expect(() => AutomatedAttendanceSystem.RetrieveAttendanceTimes(), Throws.Nothing);
            Expect(() => AutomatedAttendanceSystem.RetrieveAttendanceTimes(), Is.Not.Empty);
            
        }
        [Test]
        public void RetrieveAttendaceTimes_WhenCalled_EmployeeMemberIsAccessible()
        {
            //Data
            var employee = CommonTestCaseSourceProvider.newEmployeeSample();
            var attendanceTime = CommonTestCaseSourceProvider.newAttendanceTimeSample(employee);

            //Tests
            Expect(() => employee = AutomatedAttendanceSystem.CreateEmployee(employee), Throws.Nothing);
            Expect(() => attendanceTime = PersistenceObjectRepository<AttendanceTime>.Create(attendanceTime), Throws.Nothing);
            
            //Is not empty
            Expect(() => AutomatedAttendanceSystem.RetrieveAttendanceTimes(), Throws.Nothing);
            Expect(() => AutomatedAttendanceSystem.RetrieveAttendanceTimes(), Is.Not.Empty);

            //Employee is accesisble
            Expect(() => AutomatedAttendanceSystem.RetrieveAttendanceTimes()[0].Employee, Is.Not.Null);
            Expect(() => AutomatedAttendanceSystem.RetrieveAttendanceTimes()[0].Employee, Is.EqualTo(employee));
        }
        #endregion



    }
}
