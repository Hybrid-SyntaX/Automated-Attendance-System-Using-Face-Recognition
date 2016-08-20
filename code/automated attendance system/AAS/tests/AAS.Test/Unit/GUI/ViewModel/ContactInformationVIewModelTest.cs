using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AAS.ManagementClient.ViewModel.Test.Unit
{
    public class ContactInformationVIewModelTest:AssertionHelper
    {
        #region Test Data
        ContactInformationViewModel contactInformationVIewModelFixture; 
        #endregion
    
        #region SetUp
        [SetUp]
        public void SetUp()
        {
            contactInformationVIewModelFixture = new ContactInformationViewModel();
        } 
        #endregion
   
        #region Label
        [Test]
        public void Label_WithValidInput_ReturnsValidOutput()
        {
            string input = "Test";
            Expect(() => contactInformationVIewModelFixture.Label = input, Throws.Nothing);
            Expect(() => contactInformationVIewModelFixture.Label, Is.EqualTo(input));
        } 
        #endregion

        #region PhoneNumber
        [Test]
        public void PhoneNumber_WithValidInput_ReturnsValidOutput()
        {
            string input = "02166451223";
            Expect(() => contactInformationVIewModelFixture.PhoneNumber = input, Throws.Nothing);
            Expect(() => contactInformationVIewModelFixture.PhoneNumber, Is.EqualTo(input));
        }
        #endregion

        #region CellphoneNumber
        [Test]
        public void CellphoneNumber_WithValidInput_ReturnsValidOutput()
        {
            string input = "09126021562";
            Expect(() => contactInformationVIewModelFixture.CellphoneNumber = input, Throws.Nothing);
            Expect(() => contactInformationVIewModelFixture.CellphoneNumber, Is.EqualTo(input));
        }
        #endregion

        #region Email
        [Test]
        public void Email_WithValidInput_ReturnsValidOutput()
        {
            string input = "username@example.com";
            Expect(() => contactInformationVIewModelFixture.Email = input, Throws.Nothing);
            Expect(() => contactInformationVIewModelFixture.Email, Is.EqualTo(input));
        }
        #endregion

        #region Address
        [Test]
        public void Address_WithValidInput_ReturnsValidOutput()
        {
            string input = "Tehran, No 4";
            Expect(() => contactInformationVIewModelFixture.Address = input, Throws.Nothing);
            Expect(() => contactInformationVIewModelFixture.Address, Is.EqualTo(input));
        }
        #endregion

        #region PostalCode
        [Test]
        public void PostalCode_WithValidInput_ReturnsValidOutput()
        {
            string input = "4568794563";
            Expect(() => contactInformationVIewModelFixture.PostalCode = input, Throws.Nothing);
            Expect(() => contactInformationVIewModelFixture.PostalCode, Is.EqualTo(input));
        }
        #endregion


        #region ResetCommand
        [Test]
        public void ResetCommand_WhenAccessed_CreatesAndReturnsCommand()
        {
            Expect(() => contactInformationVIewModelFixture.ResetCommand, Is.Not.Null);
        }
        [Test]
        public void ResetCommand_WhenCanExecuteMethodIsCalled_ReturnsTrue()
        {
            Expect(() => contactInformationVIewModelFixture.ResetCommand.CanExecute(null), Is.True);
        }
        [Test]
        public void ResetCommand_WhenExecuteMethodIsCalled_CurrentContactInformationIsReset()
        {
            contactInformationVIewModelFixture.ResetCommand.Execute(null);
            Expect(contactInformationVIewModelFixture.Label,Is.EqualTo(null));
        }
        #endregion


    }
}
