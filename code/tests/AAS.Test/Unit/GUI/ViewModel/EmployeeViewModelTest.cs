using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AAS.Entities;
using AAS.Entities.Test;
using AAS.Test;
using GalaSoft.MvvmLight.Command;
using System.Drawing;
namespace AAS.ManagementClient.ViewModel.Test.Unit
{
    public class EmployeeViewModelTest : AssertionHelper
    {
        #region Test Data
        EmployeeViewModel employeeModelViewFixture;
        EmployeeViewModel employee_Ahmad_Rahimi_0004000001;
        #endregion

        #region SetUp & TearDown
        [SetUp]
        public void SetUp()
        {
            employeeModelViewFixture = new EmployeeViewModel();
            employee_Ahmad_Rahimi_0004000001 = new EmployeeViewModel()
            {
                FirstName = "Ahmad",
                LastName = "Rahimi",
                Gender = Gender.Male,
                DateOfBirth = new DateTime(1980, 01, 01),
                IdentityNumber = "13654",
                NationalID = new IRNationalID("000-000004-3"),
                ProfilePicture = new Bitmap(@"MockFiles\image_300x300_bg0_fgf.jpg"),

                //DateOfEmployement = new DateTime(2000, 04, 12),
                //EmployeeID = new EmployeeID(0, 4, 1),
            };
        }
        public void TearDown()
        {
            Persistence.PersistenceObjectRepository<Employee>.Truncate();
        }
                System.Diagnostics.Process AASServer;
        #endregion


        #region FirstName
        [Test, TestCase("Test")]
        public void FirstName_WithValidInput_ReturnsValidOutput(string input)
        {
            Expect(() => employeeModelViewFixture.FirstName = input, Throws.Nothing);
            Expect(() => employeeModelViewFixture.FirstName = input, Is.EqualTo(input));
        }
        #endregion

        #region LastName
        [Test, TestCase("Test")]
        public void LastName_WithValidInput_ReturnsValidOutput(string input)
        {
            Expect(() => employeeModelViewFixture.LastName = input, Throws.Nothing);
            Expect(() => employeeModelViewFixture.LastName = input, Is.EqualTo(input));
        }
        #endregion

        #region IdentityNumber
        [Test, TestCase("4564")]
        public void IdentityNumber_WithValidInput_ReturnsValidOutput(string input)
        {
            Expect(() => employeeModelViewFixture.IdentityNumber = input, Throws.Nothing);
            Expect(() => employeeModelViewFixture.IdentityNumber = input, Is.EqualTo(input));
        }
        #endregion

        #region Gender
        [Test, TestCase(Gender.Male), TestCase(Gender.Female)]
        public void Gender_WithValidInput_ReturnsValidOutput(Gender input)
        {
            Expect(() => employeeModelViewFixture.Gender = input, Throws.Nothing);
            Expect(() => employeeModelViewFixture.Gender = input, Is.EqualTo(input));
        }
        #endregion

        #region NationalID

        [Test]
        public void NationalID_WithValidInput_ReturnsValidOutput()
        {
            IRNationalID input = new IRNationalID("000-000004-3");
            Expect(() => employeeModelViewFixture.NationalID = input, Throws.Nothing);
            Expect(() => employeeModelViewFixture.NationalID = input, Is.EqualTo(input));
        }
        #endregion

        #region DateOfBirth
        [Test]
        public void DateOfBirth_WithValidInput_ReturnsValidOutput()
        {
            DateTime input = new DateTime(1990, 02, 09);
            Expect(() => employeeModelViewFixture.DateOfBirth = input, Throws.Nothing);
            Expect(() => employeeModelViewFixture.DateOfBirth = input, Is.EqualTo(input));
        }
        #endregion

        //#region DateOfBirth
        //[Test]
        //public void ProfilePicture_WithValidInput_ReturnsValidOutput()
        //{
        //    ProfilePicture input = new ProfilePicture(MockFiles.image_300x300_bg0_fgf_Path);
        //    Expect(() => employeeModelViewFixture.ProfilePicture = input, Throws.Nothing);
        //    Expect(() => employeeModelViewFixture.ProfilePicture = input, Is.EqualTo(input));
        //}
        //#endregion


        #region SaveCommand
        [Test]
        public void SaveCommand_WhenAccessed_CreatesAndReturnsCommand()
        {
            Expect(() => employeeModelViewFixture.SaveCommand, Is.Not.Null);
        }
        [Test]
        public void SaveCommand_WhenCanExecuteMethodIsCalled_ReturnsTrue()
        {
            Expect(() => employeeModelViewFixture.SaveCommand.CanExecute(null), Is.True);
        }
        [Test]
        public void SaveCommand_WhenExecuteMethodIsCalled_CreatesNewEmployee()
        {
            employee_Ahmad_Rahimi_0004000001.SaveCommand.Execute(null);
            Expect(() => Persistence.PersistenceObjectRepository<Employee>.RetrieveAll().Count, Is.EqualTo(1));
        }
        #endregion
        #region SetProfilePictureCommand
        [Test]
        public void SetProfilePictureCommand_WhenAccessed_CreatesAndReturnsCommand()
        {
            Expect(() => employeeModelViewFixture.SetProfilePictureCommand, Is.Not.Null);
        }
        [Test]
        public void SetProfilePictureCommand_WhenCanExecuteMethodIsCalled_ReturnsTrue()
        {
            Expect(() => employeeModelViewFixture.SetProfilePictureCommand.CanExecute(null), Is.True);
        }
        [Test]
        public void SetProfilePictureCommand_WhenExecuteMethodIsCalled_CreatesNewEmployee()
        {

            AAS.Test.Util.Mocker.MockPrivateField(typeof(EmployeeViewModel), employeeModelViewFixture, "m_setProfilePictureCommand",
            new RelayCommand(() =>
            {
                employeeModelViewFixture.ProfilePicture = new Bitmap(MockFiles.image_300x300_bg0_fgf_Path);
            }),
            () =>
            {
                employeeModelViewFixture.SetProfilePictureCommand.Execute(null);
                Expect(() => employeeModelViewFixture.ProfilePicture, Is.Not.Null);
            });

        }
        #endregion


        #region DeleteCommand
        [Test]
        public void DeleteCommand_WhenAccessed_CreatesAndReturnsCommand()
        {
            Expect(() => employee_Ahmad_Rahimi_0004000001.DeleteCommand, Is.Not.Null);
        }
        [Test]
        public void DeleteCommand_WhenCanExecuteMethodIsCalled_ReturnsTrue()
        {
            Expect(() => employee_Ahmad_Rahimi_0004000001.DeleteCommand.CanExecute(null), Is.True);
        }
        [Test]
        public void DeleteCommand_WhenExecuteMethodIsCalled_EmployeeInstanceIsDeleted()
        {
            Expect(() => employee_Ahmad_Rahimi_0004000001.SaveCommand.Execute(null), Throws.Nothing);

            Employee employeeInstance = getEmployeeInstance(employee_Ahmad_Rahimi_0004000001);

            Expect(() => employee_Ahmad_Rahimi_0004000001.DeleteCommand.Execute(null), Throws.Nothing);

            Expect(Persistence.PersistenceObjectRepository<Employee>.Retrieve(employeeInstance.ID), Is.Null);
        }

        private Employee getEmployeeInstance(EmployeeViewModel employeeViewModel)
        {
            Employee employeeInstance = (Employee)AAS.Test.Util.Mocker.GetPrivateFieldValue(typeof(EmployeeViewModel), employeeViewModel, "m_employee");
            return employeeInstance;
        }
        #endregion


        #region ResetCommand
        [Test]
        public void ResetCommand_WhenAccessed_CreatesAndReturnsCommand()
        {
            Expect(() => employee_Ahmad_Rahimi_0004000001.ResetCommand, Is.Not.Null);
        }
        [Test]
        public void ResetCommand_WhenCanExecuteMethodIsCalled_ReturnsTrue()
        {
            Expect(() => employee_Ahmad_Rahimi_0004000001.ResetCommand.CanExecute(null), Is.True);
        }
        [Test]
        public void ResetCommand_WhenExecuteMethodIsCalled_EmployeePersonalInformationIsReset()
        {
            employee_Ahmad_Rahimi_0004000001.ResetCommand.Execute(null);
            Expect(employee_Ahmad_Rahimi_0004000001.FirstName, Is.Null);
        }
        #endregion


        #region ResetAllCommand
        [Test]
        public void ResetAllCommand_WhenAccessed_CreatesAndReturnsCommand()
        {
       //     Expect(() => employee_Ahmad_Rahimi_0004000001.ResetAllCommand, Is.Not.Null);
        }
        [Test]
        public void ResetAllCommand_WhenCanExecuteMethodIsCalled_ReturnsTrue()
        {
       //     Expect(() => employee_Ahmad_Rahimi_0004000001.ResetAllCommand.CanExecute(null), Is.True);
        }
        [Test,Ignore]
        public void ResetAllCommand_WhenExecuteMethodIsCalled_EmployeeIsReset()
        {
            //Mock
            Employee employeeInstance = getEmployeeInstance(employee_Ahmad_Rahimi_0004000001);
            employeeInstance.WorkSchedule = WorkSchedule.MinValue;
            employeeInstance.ContactInformations.Add(CommonTestCaseSourceProvider.newContactInformationSample());

            //Test
         //   Expect(() => employee_Ahmad_Rahimi_0004000001.ResetAllCommand.Execute(null), Throws.Nothing);
            Expect(() => employee_Ahmad_Rahimi_0004000001.FirstName, Is.Null);
            Expect(() => employeeInstance.WorkSchedule, Is.EqualTo(WorkSchedule.MinValue));
            Expect(() => employeeInstance.ContactInformations[0].Label, Is.EqualTo(null));
            Expect(() => employeeInstance.ContactInformations.Count, Is.EqualTo(0));
        }
        #endregion




    }
}
