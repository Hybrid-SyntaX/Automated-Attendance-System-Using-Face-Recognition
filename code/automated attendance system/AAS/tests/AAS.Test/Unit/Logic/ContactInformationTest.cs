using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AAS.Entities;
using AAS.Entities.Exceptions;
namespace AAS.Entities.Test.Unit
{
    public class ContactInformationTest : AssertionHelper
    {

        #region Test Data
        public string[] InvalidPostalCode = { "113456789", "11345678913", "1323456781", "1003456781" };
        public string[] InvalidEmail = { "a@b.c", "@b.co", "b.co", "ab@ada@adad.com", "abcd#ef@gh.com" };
        ContactInformation contactInformationFixture;
        ContactInformation contactInformationFixture_Home;
        #endregion
        #region SetUp
        [SetUp]
        public void SetUp()
        {
            contactInformationFixture = new ContactInformation();
            contactInformationFixture_Home = new ContactInformation()
            {
                ID = Guid.NewGuid(),
                Label = "Home",
                PhoneNumber = "02199115532",
                CellphoneNumber = "09126512321",
                Email = "aaa@bbb.com",
                Address = "No 3, Valiasr, Tehran",
                PostalCode = "1434567891",
            };
        }
        #endregion

        #region Label
        [Test, TestCaseSource(typeof(CommonTestCaseSourceProvider), "NullOrEmptyOrWhitespace")]
        public void Label_IsNullOrEmptyOrWhitespace_ThrowsRequiredFieldException(string input)
        {
            Expect(() => contactInformationFixture.Label = input, Throws.Exception.TypeOf<RequiredFieldException>());
        }
        [Test, TestCaseSource(typeof(CommonTestCaseSourceProvider), "InvalidAlphabet")]
        public void Label_HasValidFormat_ThrowsInvalidFormatException(string illegalInput)
        {
            Expect(() => contactInformationFixture.Label = illegalInput, Throws.Exception.TypeOf<InvalidFormatException>());
        }
        [Test]
        public void Label_SetterGetter()
        {
            contactInformationFixture.Label = "ABCDE";
            Expect(contactInformationFixture.Label, Is.EqualTo("ABCDE"));
        }
        #endregion
        #region Address
        [Test, TestCaseSource(typeof(CommonTestCaseSourceProvider), "NullOrEmptyOrWhitespace")]
        public void Address_IsNullOrEmptyOrWhitespace_ThrowsRequiredFieldException(string input)
        {
            Expect(() => contactInformationFixture.Address = input, Throws.Exception.TypeOf<RequiredFieldException>());
        }
        [Test]
        public void Address_LengthIsLessThan5_ThrowsInputTooShortException()
        {
            Expect(() => contactInformationFixture.Address = "abcd", Throws.Exception.TypeOf<InputTooShortException>());
        }
        [Test]
        public void Address_SetterGetter()
        {
            contactInformationFixture.Address = "ABCDE";
            Expect(contactInformationFixture.Address, Is.EqualTo("ABCDE"));
        }
        #endregion
        #region PhoneNumber
        [Test, TestCaseSource(typeof(CommonTestCaseSourceProvider), "NullOrEmptyOrWhitespace")]
        public void PhoneNumber_IsNullOrEmptyOrWhitespace_ThrowsRequiredFieldException(string input)
        {
            Expect(() => contactInformationFixture.PhoneNumber = input, Throws.Exception.TypeOf<RequiredFieldException>());
        }
        [Test, TestCaseSource(typeof(CommonTestCaseSourceProvider), "InvalidDigit")]
        public void PhoneNumber_hasInvalidFormat_ThrowsInvalidFormatException(string invalidInput)
        {
            Expect(() => contactInformationFixture.PhoneNumber = invalidInput, Throws.Exception.TypeOf<InvalidFormatException>());
        }
        [Test, TestCase("02166112132"), TestCase("+982166112132"), TestCase("212166112132"), TestCase("+9812166112132")]
        public void PhoneNumber_SetterGetter(string validInput)
        {
            contactInformationFixture.PhoneNumber = validInput;
            Expect(contactInformationFixture.PhoneNumber, Is.EqualTo(validInput));
        }
        #endregion
        #region CellPhoneNumber
        [Test, TestCaseSource(typeof(CommonTestCaseSourceProvider), "NullOrEmptyOrWhitespace")]
        public void CellphoneNumber_IsNullOrEmptyOrWhitespace_ThrowsRequiredFieldException(string input)
        {
            Expect(() => contactInformationFixture.CellphoneNumber = input, Throws.Exception.TypeOf<RequiredFieldException>());
        }
        [Test, TestCaseSource(typeof(CommonTestCaseSourceProvider), "InvalidDigit")]
        public void CellphoneNumber_hasInvalidFormat_ThrowsInvalidFormatException(string invalidInput)
        {
            Expect(() => contactInformationFixture.CellphoneNumber = invalidInput, Throws.Exception.TypeOf<InvalidFormatException>());
        }
        [Test, TestCase("09128024526"), TestCase("+989128024526")]
        public void CellphoneNumber_SetterGetter(string validInput)
        {
            contactInformationFixture.CellphoneNumber = validInput;
            Expect(contactInformationFixture.CellphoneNumber, Is.EqualTo(validInput));
        }
        #endregion
        #region Email
        [Test, TestCaseSource(typeof(CommonTestCaseSourceProvider), "NullOrEmptyOrWhitespace")]
        public void Email_IsNullOrEmptyOrWhitespace_ThrowsRequiredFieldException(string input)
        {
            Expect(() => contactInformationFixture.Email = input, Throws.Exception.TypeOf<RequiredFieldException>());
        }
        [Test, TestCaseSource("InvalidEmail")]
        public void Email_hasInvalidFormat_ThrowsInvalidFormatException(string input)
        {
            Expect(() => contactInformationFixture.Email = input, Throws.Exception.TypeOf<InvalidFormatException>());
        }
        [Test, TestCase("abb.2005@gmail.com"), TestCase("abb2005@gmail.com"), TestCase("abb2005@abc.gmail.com")]
        public void Email_SetterGetter(string validInput)
        {
            contactInformationFixture.Email = validInput;
            Expect(contactInformationFixture.Email, Is.EqualTo(validInput));
        }
        #endregion
        #region PostalCode
        [Test, TestCaseSource(typeof(CommonTestCaseSourceProvider), "NullOrEmptyOrWhitespace")]
        public void PostalCode_IsNullOrEmptyOrWhitespace_ThrowsRequiredFieldException(string input)
        {
            Expect(() => contactInformationFixture.PostalCode = input, Throws.Exception.TypeOf<RequiredFieldException>());
        }
        [Test, TestCaseSource("InvalidPostalCode")]
        public void PostalCode_hasInvalidFormat_ThrowsInvalidFormatException(string input)
        {
            Expect(() => contactInformationFixture.PostalCode = input, Throws.Exception.TypeOf<InvalidFormatException>());
        }
        [Test, TestCase("3134567891")]
        public void PostalCode_SetterGetter(string validInput)
        {
            contactInformationFixture.PostalCode = validInput;
            Expect(contactInformationFixture.PostalCode, Is.EqualTo(validInput));
        }
        #endregion
        #region ID
        [Test]
        public void ID_IsSet_Passes()
        {
            Expect(() => contactInformationFixture.ID = Guid.NewGuid(), Is.Not.EqualTo(Guid.Empty));
        }
        [Test]
        public void ID_GetterSetter()
        {
            Guid guid = Guid.Parse("{66956516-13E3-46A9-8E51-F97C2E06E2BD}");
            contactInformationFixture.ID = guid;
            Expect(contactInformationFixture.ID, Is.EqualTo(Guid.Parse("{66956516-13E3-46A9-8E51-F97C2E06E2BD}")));
        }
        [Test]
        public void ID_IsEmpty_ThrowsRequiredFieldExceptionException()
        {
            Expect(() => contactInformationFixture.ID = Guid.Empty, Throws.Exception.TypeOf<RequiredFieldException>());
        }
        #endregion
        #region Employee
        [Test]
        public void Employee_IsNotSet_ThrowsRequiredFieldException()
        {
            Expect(() => contactInformationFixture.Employee = null, Throws.Exception.TypeOf<RequiredFieldException>());
        }
        [Test]
        public void Employee_SetterGetter()
        {
            Employee employee = new Employee();
            contactInformationFixture.Employee = employee;
            Expect(contactInformationFixture.Employee, Is.EqualTo(employee));
        }
        #endregion
        #region Equals
        [Test]
        public void Equals_IsEqual_Passes()
        {
            Guid guid = Guid.NewGuid();
            ContactInformation A = new ContactInformation();
            A.ID = guid;
            ContactInformation B = new ContactInformation();
            B.ID = guid;
            Expect(A.Equals(B), Is.True);
        }
        [Test]
        public void Equals_IsNotEqual_Fails()
        {
            ContactInformation A = new ContactInformation();
            A.ID = Guid.NewGuid();
            ContactInformation B = new ContactInformation();
            B.ID = Guid.NewGuid();
            Expect(A.Equals(B), Is.False);
        }
        [Test]
        public void Equals_InputIsNull_Fails()
        {
            ContactInformation A = new ContactInformation();

            Expect(A.Equals(null), Is.False);
        }
        #endregion
        #region Clone
        [Test]
        public void Clone_ClonedContactInformation_IsEqualToOriginalInstance()
        {

            Expect(() => contactInformationFixture_Home.Clone(), Is.EqualTo(contactInformationFixture_Home));
        }
        #endregion
        #region GetHashCode
        [Test]
        public void GetHashCode_ForHome_Passes()
        {
            Expect(contactInformationFixture_Home.GetHashCode(), Is.EqualTo(contactInformationFixture_Home.Clone().GetHashCode()));
        }
        #endregion
        #region == and != Operators
        [Test]
        public void EqualOperator_WithEqualInput_ReturnsTrue()
        {
            ContactInformation A = contactInformationFixture_Home;
            ContactInformation B = contactInformationFixture_Home;
            Expect(() => A == B, Is.True);
        }
        [Test]
        public void EqualOperator_WithUnequalInput_ReturnsFalse()
        {
            ContactInformation A = contactInformationFixture_Home;
            ContactInformation B = contactInformationFixture;
            Expect(() => A == B, Is.False);
        }
        [Test]
        public void EqualOperator_WithNullInput_ReturnsFalse()
        {
            ContactInformation A = null;
            ContactInformation B = new ContactInformation();
            Expect(() => A == B, Is.False);
        }
        [Test]
        public void UnequalOperator_WithNullInput_ReturnsTrue()
        {
            ContactInformation A = null;
            ContactInformation B = new ContactInformation();
            Expect(() => A != B, Is.True);
        }
        [Test]
        public void UnequalOperator_WithUnequalInput_ReturnsTrue()
        {

            ContactInformation A = new ContactInformation();
            ContactInformation B = contactInformationFixture_Home;
            Expect(() => A != B, Is.True);
        }
        [Test]
        public void UnequalOperator_WithEqualInput_ReturnsFalse()
        {
            ContactInformation A = contactInformationFixture_Home;
            ContactInformation B = contactInformationFixture_Home;
            Expect(() => A != B, Is.False);
        }
        #endregion
        #region ToString
        [Test]
        public void ToString_WhenCalled_ReturnsIDString()
        {
            Expect(contactInformationFixture_Home.ToString(),Is.EqualTo(contactInformationFixture_Home.ID.ToString()));
        }
        #endregion

    }
}
