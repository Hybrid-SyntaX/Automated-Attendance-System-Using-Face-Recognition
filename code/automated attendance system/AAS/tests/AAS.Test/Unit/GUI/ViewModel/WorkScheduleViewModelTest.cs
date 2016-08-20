using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AAS.Entities;
namespace AAS.ManagementClient.ViewModel.Test.Unit
{
    public class WorkScheduleViewModelTest:AssertionHelper
    {
        #region SetUp & Test Data
    //    WorkScheduleViewModel workScheduleViewModelFixture;

        [SetUp]
        public void SetUp()
        {
     //       workScheduleViewModelFixture = new WorkScheduleViewModel();
        } 
        #endregion

        #region SetWorkDayCommand
        [Test]
        public void SetWorkDayCommand_WhenAccessed_CreatesAndReturnsCommand()
        {
          //  Expect(() => workScheduleViewModelFixture.SetWorkDayCommand, Is.Not.Null);
        }
        [Test]
        public void SetWorkDayCommand_WhenCanExecuteMethodIsCalled_ReturnsTrue()
        {
           // Expect(() => workScheduleViewModelFixture.SetWorkDayCommand.CanExecute(null), Is.True);
        }
        [Test]
        public void SetWorkDayCommand_WhenExecuteMethodIsCalled_WorkDayIsSet()
        {
     //       workScheduleViewModelFixture.SetWorkDayCommand.Execute(new int []{1,0});
       //     WorkSchedule workSchedule_Saturday00=WorkSchedule.Parse("800000-0-0-0-0-0-0");
         //   Expect(AAS.Test.Util.Mocker.GetPrivateFieldValue(typeof(WorkScheduleViewModel), workScheduleViewModelFixture, "m_workschedule"), Is.EqualTo(workSchedule_Saturday00));
        }
        #endregion
        #region SetOffDayCommand
        [Test]
        public void SetOffDayCommand_WhenAccessed_CreatesAndReturnsCommand()
        {
         //   
        //    Expect(() => workScheduleViewModelFixture.SetOffDayCommand, Is.Not.Null);
        }
        [Test]
        public void SetOffDayCommand_WhenCanExecuteMethodIsCalled_ReturnsTrue()
        {
          //  Expect(() => workScheduleViewModelFixture.SetOffDayCommand.CanExecute(null), Is.True);
        }
        [Test]
        public void SetOffDayCommand_WhenExecuteMethodIsCalled_WorkDayIsSet()
        {
         //   workScheduleViewModelFixture.SetOffDayCommand.Execute(new int[] { 1, 0 });
           // Expect(AAS.Test.Util.Mocker.GetPrivateFieldValue(typeof(WorkScheduleViewModel), workScheduleViewModelFixture, "m_workschedule"), Is.EqualTo(WorkSchedule.MinValue));
        }
        #endregion

    }
}
