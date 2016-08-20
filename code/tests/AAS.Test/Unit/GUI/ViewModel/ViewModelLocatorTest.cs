using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
namespace AAS.ManagementClient.ViewModel.Test.Unit
{
    public class ViewModelLocatorTest:AssertionHelper
    {
        #region Test Data
        ViewModelLocator viewModelLocatorFixture;
        #endregion
        #region SetUp
        [SetUp]
        public void SetUp()
        {
            viewModelLocatorFixture = new ViewModelLocator();
        }
        #endregion


        #region EmployeeViewModel
        [Test]
        public void EmployeeViewModel_WhenAccessed_ReturnsEmployeeViewModelInstance()
        {
            Expect(viewModelLocatorFixture.EmployeeViewModel,Is.InstanceOf<EmployeeViewModel>());
        }
        #endregion

        #region ContactInformationViewModel
        [Test]
        public void ContactInformationViewModel_WhenAccessed_ReturnsContactInformationViewModelInstance()
        {
            Expect(viewModelLocatorFixture.ContactInformationViewModel, Is.InstanceOf<ContactInformationViewModel>());
        }
        #endregion

        #region WorkScheduleViewModel
        [Test]
        public void WorkScheduleViewModel_WhenAccessed_ReturnsWorkScheduleViewModelInstance()
        {
        //    Expect(viewModelLocatorFixture.WorkScheduleViewModel, Is.InstanceOf<WorkScheduleViewModel>());
        }
        #endregion
    }
}
