using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAS.Persistence;
using NUnit.Framework;
using AAS.Entities;
using AAS.Persistence.Mappings;
using System.Drawing;
using System.Diagnostics;
using NHibernate.Tool.hbm2ddl;
namespace AAS.Persistence.Test.Unit
{

    public class PersistenceObjectRepositoryTest : AssertionHelper
    {
        #region Test Data
        List<Type> Types = new List<Type> { typeof(EmployeeMap), typeof(ContactInformationMap) };

        public Employee newEmployeeSample(ContactInformation contactInformation = null)
        {
            Employee employee = new Employee()
            {
                FirstName = "Ahmad",
                LastName = "Rahimi",
                Gender = Gender.Male,
                DateOfBirth = new DateTime(1980, 01, 01),
                DateOfEmployement = new DateTime(2000, 04, 12),
                IdentityNumber = "13654",
                NationalID = new IRNationalID("000-000004-3"),
                EmployeeID = new EmployeeID(0, 4, 1),
                ProfilePicture = new Bitmap(@"MockFiles\image_300x300_bg0_fgf.jpg"),
            };
            if (contactInformation != null)
                employee.ContactInformations.Add(contactInformation);
            return employee;
        }
        private ContactInformation newContactInformationSample()
        {
            ContactInformation contactInformation = new ContactInformation()
            {
                Label = "Home",
                PhoneNumber = "02199115532",
                CellphoneNumber = "09126512321",
                Email = "aaa@bbb.com",
                Address = "No 3, Valiasr, Tehran",
                PostalCode = "1434567891",
            };

            //contactInformation.Employee = newEmployeeSample();
            return contactInformation;
        }
        #endregion

        #region SetUp & TearDown
        [SetUp]
        public void SetUp()
        {
            NHibernateHelper.InitializeInMemoryDatabase(Types);
        }
        [TearDown]
        public void TearDown()
        {
            AAS.Test.Util.Mocker.CallPrivateStaticMethod(typeof(NHibernateHelper), "reset", null);
        }
        #endregion

        #region Create
        [Test]
        public void Create_ProvidedWithEmployee_SavesInDatabase()
        {
            var returnValue = PersistenceObjectRepository<Employee>.Create(newEmployeeSample());
            Expect(() => returnValue.ID, Is.Not.EqualTo(Guid.Empty));
        }
        [Test]
        public void Create_ProvidedWithContactInformation_SavesInDatabase()
        {
            var returnValue = PersistenceObjectRepository<ContactInformation>.Create(newContactInformationSample());
            Expect(() => returnValue.ID, Is.Not.EqualTo(Guid.Empty));
        }
        #endregion

        #region Retrieve
        [Test]
        public void Retrieve_ProvidedWithID_ReturnsEmployeeInstance()
        {
            var returnValue = PersistenceObjectRepository<Employee>.Create(newEmployeeSample());
            Expect(() => PersistenceObjectRepository<Employee>.Retrieve(returnValue.ID), Is.Not.Null);
        }
        [Test]
        public void Retrieve_ProvidedWithID_ReturnsContactInformationInstance()
        {
            var contactInformation = PersistenceObjectRepository<ContactInformation>.Create(newContactInformationSample());
            var employee = PersistenceObjectRepository<Employee>.Create(newEmployeeSample(contactInformation));

            Expect(() => PersistenceObjectRepository<ContactInformation>.Retrieve(contactInformation.ID), Is.Not.EqualTo(Guid.Empty));
        }
        [Test]
        public void Retrieve_WhenCalledOnEmployeeWithNonExistingID_ReturnsNull()
        {
            Expect(() => PersistenceObjectRepository<Employee>.Retrieve(Guid.NewGuid()),Is.Null);
        }
        public void Retrieve_WhenCalledOnContactInformationWithNonExistingID_ReturnsNull()
        {
            Expect(() => PersistenceObjectRepository<ContactInformation>.Retrieve(Guid.NewGuid()), Is.Null);
        }
        #endregion

        #region Retrieve Employee with ContactInformations
        [Test]
        public void Employee_WhenContactInformationsIsAccessed_ContactInformationsAreRetrieved()
        {
            var contactInformation = PersistenceObjectRepository<ContactInformation>.Create(newContactInformationSample());
            Guid id=PersistenceObjectRepository<Employee>.Create(newEmployeeSample(contactInformation)).ID;
            
            Employee employee;
            Expect(employee = PersistenceObjectRepository<Employee>.Retrieve(id), Is.Not.Null);
            
            Expect(() => employee.ContactInformations.Count, Is.EqualTo(1));
        }
        #endregion

        #region Delete
        [Test]
        public void Delete_WhenCalled_EmployeeIsDeleted()
        {
            var returnValue = PersistenceObjectRepository<Employee>.Create(newEmployeeSample());
            PersistenceObjectRepository<Employee>.Delete(returnValue);

            Guid removedId = returnValue.ID;
            Expect(() => PersistenceObjectRepository<Employee>.Retrieve(removedId), Is.Null);

        }
        [Test]
        public void Delete_WhenCalled_ContactInformationIsDeleted()
        {
            var returnValue = PersistenceObjectRepository<ContactInformation>.Create(newContactInformationSample());
            PersistenceObjectRepository<ContactInformation>.Delete(returnValue);

            Guid removedId = returnValue.ID;
            Expect(() => PersistenceObjectRepository<ContactInformation>.Retrieve(removedId), Is.Null);
        }
        #endregion

        #region Update
        [Test]
        public void Update_WhenCalled_EmployeeIsUpdated()
        {
            Employee returnValue = PersistenceObjectRepository<Employee>.Create(newEmployeeSample());
            returnValue.FirstName = "Mohammad";
            Employee updatedValue = PersistenceObjectRepository<Employee>.Update(returnValue);
            Expect(updatedValue.FirstName, Is.EqualTo(returnValue.FirstName));
        }
        [Test]
        public void Update_WhenCalled_ContactInformationIsUpdated()
        {
            ContactInformation returnValue = PersistenceObjectRepository<ContactInformation>.Create(newContactInformationSample());
            returnValue.Label = "Work";
            ContactInformation updatedValue = PersistenceObjectRepository<ContactInformation>.Update(returnValue);
            Expect(updatedValue.Label, Is.EqualTo(returnValue.Label));
        }
        #endregion

        #region RetrieveAll
        [Test]
        public void RetrieveAll_WhenCalled_ReturnsEmployeesList()
        {
            for (int i = 0; i < 2; i++)
                PersistenceObjectRepository<Employee>.Create(newEmployeeSample());
            Expect(PersistenceObjectRepository<Employee>.RetrieveAll().Count, Is.EqualTo(2));
        }
        [Test]
        public void RetrieveAll_WhenCalled_ReturnsContactInformationList()
        {
            for (int i = 0; i < 2; i++)
            {
                var contactInformation = PersistenceObjectRepository<ContactInformation>.Create(newContactInformationSample());
                var employee = PersistenceObjectRepository<Employee>.Create(newEmployeeSample(contactInformation));
            }

            Expect(PersistenceObjectRepository<ContactInformation>.RetrieveAll().Count, Is.EqualTo(2));
        }
        #endregion

        #region Truncate
        [Test]
        public void Truncate_WhenCalledOnEmployee_DeletesAllEmployees()
        {
            PersistenceObjectRepository<Employee>.Create(newEmployeeSample());
            
            
            PersistenceObjectRepository<Employee>.Truncate();
            Expect(() => PersistenceObjectRepository<Employee>.RetrieveAll().Count, Is.EqualTo(0));
        }
        [Test]
        public void Truncate_WhenCalledOnContactInformation_DeletesAllContactInformations()
        {
            var contactInformation=PersistenceObjectRepository<ContactInformation>.Create(newContactInformationSample());
            var employee = PersistenceObjectRepository<Employee>.Create(newEmployeeSample(contactInformation));

            PersistenceObjectRepository<ContactInformation>.Truncate();
            Expect(() => PersistenceObjectRepository<ContactInformation>.RetrieveAll().Count, Is.EqualTo(0));
        }
        #endregion


    }

}
