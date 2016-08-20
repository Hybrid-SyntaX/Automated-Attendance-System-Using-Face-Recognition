using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using NUnit.Framework;
using AAS.Entities;
using AAS.Entities.Exceptions;
using Rhino.Mocks;
using System.Drawing;
namespace AAS.Entities.Test.Unit
{
    [TestFixture]
    public class EmployeeTest : AssertionHelper
    {


        #region Test Data
        Employee employeeFixture;
        Employee employee_AhmadRahimi_0004000001;
        #endregion
        #region SetUp
        [SetUp]
        public void SetUp()
        {
            employeeFixture = new Employee();
            employee_AhmadRahimi_0004000001 = new Employee()
            {
                ID = Guid.NewGuid(),
                FirstName = "Ahmad",
                LastName = "Rahimi",
                Gender = Gender.Male,
                DateOfBirth = new DateTime(1980, 01, 01),
                DateOfEmployement = new DateTime(2000, 04, 12),
                IdentityNumber = "13654",
                NationalID = new IRNationalID("000-000004-3"),
                EmployeeID = new EmployeeID(0, 4, 1),
                ProfilePicture = new Bitmap(@"MockFiles\image_300x300_bg0_fgf.jpg"),
                WorkSchedule = WorkSchedule.MaxValue
            };
        }
        #endregion

        #region FirstName Member
        [Test, TestCaseSource(typeof(CommonTestCaseSourceProvider), "NullOrEmptyOrWhitespace")]
        public void FirstName_IsNullOrEmptyOrWhitespace_ThrowsRequiredFieldException(string invalidInput)
        {
            Expect(() => employeeFixture.FirstName = invalidInput
           , Throws.Exception.TypeOf<RequiredFieldException>());
        }
        [Test]
        public void FirstName_ContainsIllegalCharacters_ThrowsInvalidFormatException()
        {
            Expect(() => employeeFixture.FirstName = "ABC AA @ @#!#@"
            , Throws.Exception.TypeOf<InvalidFormatException>());
        }
        [Test]
        public void FirstName_ContainsSpaceBetweenWords_ShouldPass()
        {
            Expect(() => employeeFixture.FirstName = "A Name With Space"
            , Throws.Nothing);
        }
        [Test]
        public void FirstName_ContainsUnicodeCharacters_ShouldPass()
        {
            Expect(() => employeeFixture.FirstName = "اسم"
            , Throws.Nothing);
        }
        [Test]
        public void FirstName_IsSomething_ReturnsSomething()
        {
            Expect(() => employeeFixture.FirstName = "Something", Is.EqualTo("Something"));
        }
        [Test]
        public void FirstName_IsSetToSomething_ReturnsSomething()
        {
            employeeFixture.FirstName = "Something";
            Expect(employeeFixture.FirstName, Is.EqualTo("Something"));
        }
        #endregion
        #region LastName Member
        [Test, TestCaseSource(typeof(CommonTestCaseSourceProvider), "NullOrEmptyOrWhitespace")]
        public void LastName_IsNullOrEmptyOrWhitespace_ThrowsRequiredFieldException(string invalidInput)
        {
            Expect(() => employeeFixture.LastName = invalidInput
           , Throws.Exception.TypeOf<RequiredFieldException>());
        }
        [Test]
        public void LastName_ContainsIllegalCharacters_ThrowsInvalidFormatException()
        {
            Expect(() => employeeFixture.LastName = "ABC AA @ @#!#@"
            , Throws.Exception.TypeOf<InvalidFormatException>());
        }
        [Test]
        public void LastName_ContainsSpaceBetweenWords_ShouldPass()
        {
            Expect(() => employeeFixture.LastName = "A Name With Space"
            , Throws.Nothing);
        }
        [Test]
        public void LastName_ContainsUnicodeCharacters_ShouldPass()
        {
            Expect(() => employeeFixture.LastName = "اسم"
            , Throws.Nothing);
        }
        [Test]
        public void LastName_IsSomething_ReturnsSomething()
        {
            Expect(() => employeeFixture.LastName = "Something", Is.EqualTo("Something"));
        }
        [Test]
        public void LastName_IsSetToSomething_ReturnsSomething()
        {
            employeeFixture.LastName = "Something";
            Expect(employeeFixture.LastName, Is.EqualTo("Something"));
        }
        #endregion
        #region Gender Member
        [Test]
        public void Gender_IsMale_ReturnsMale()
        {
            Expect(() => employeeFixture.Gender = Gender.Male
            , Is.EqualTo(Gender.Male));
        }
        [Test]
        public void Gender_IsSetToMale_ReturnsMale()
        {
            employeeFixture.Gender = Gender.Male;
            Expect(employeeFixture.Gender
            , Is.EqualTo(Gender.Male));
        }
        #endregion
        #region DateOfBirth
        [Test]
        public void DateOfBirth_IsNotSet_ThrowsRequiredFieldException()
        {

            Expect(() => employeeFixture.DateOfBirth = new DateTime()
            , Throws.Exception.TypeOf<RequiredFieldException>());
        }
        [Test]
        public void DateOfBirth_IsGreaterThanOrEqualToToday_ThrowsInvalidDateException()
        {
            DateTime tomorrow = DateTime.Now.AddDays(+1);

            Expect(() => employeeFixture.DateOfBirth = tomorrow
            , Throws.Exception.TypeOf<InvalidDateException>());
        }
        [Test]
        public void DateOfBirth_IsLessThan18_ThrowsIllegalAgeException()
        {
            DateTime fifteenYearOld = DateTime.Today.AddYears(-15);

            Expect(() => employeeFixture.DateOfBirth = fifteenYearOld
            , Throws.Exception.TypeOf<IllegalAgeException>());

        }
        [Test]
        public void DateOfBirth_IsGreaterOrEqualThan200_ThrowsInvalidDateException()
        {
            DateTime threeHundredYearOld = DateTime.Today.AddYears(-300);

            Expect(() => employeeFixture.DateOfBirth = threeHundredYearOld
            , Throws.Exception.TypeOf<InvalidFormatException>());

        }
        [Test]
        public void DateOfBirth_IsSetTo19801010_Returns19801010()
        {
            Expect(employeeFixture.DateOfBirth = new DateTime(1980, 10, 10), Is.EqualTo(new DateTime(1980, 10, 10)));
        }

        #endregion
        #region DateOfEmployement
        [Test]
        public void DateOfEmployement_IsNotSet_ThrowsRequiredFieldException()
        {
            Expect(() => employeeFixture.DateOfEmployement = new DateTime()
            , Throws.Exception.TypeOf<RequiredFieldException>());

        }
        [Test]
        public void DateOfEmployement_IsGreaterThanToday_ThrowsInvalidDateException()
        {
            DateTime tomorrow = DateTime.Now.AddDays(+1);

            Expect(() => employeeFixture.DateOfEmployement = tomorrow
            , Throws.Exception.TypeOf<InvalidDateException>());
        }
        [Test]
        public void DateOfEmployement_IsSetTo19801010_Returns19801010()
        {
            Expect(employeeFixture.DateOfEmployement = new DateTime(1980, 10, 10), Is.EqualTo(new DateTime(1980, 10, 10)));
        }

        [Test]
        public void DateOfEmployement_IsLessThanDateOfBirth_ThrowsInvalidDateException()
        {
            employeeFixture.DateOfBirth = new DateTime(1980, 10, 10);
            Expect(() => employeeFixture.DateOfEmployement = new DateTime(1970, 10, 10)
            , Throws.Exception.TypeOf<InvalidDateException>());
        }
        #endregion
        #region NationalID
        [Test]
        public void NationalID_WasNotInitialized_ReturnsNewInstance()
        {
            Expect(employeeFixture.NationalID, Is.EqualTo(new IRNationalID()));
        }
        [Test]
        public void NationalID_IsNull_ThrowsRequiredFieldException()
        {
            Expect(() => employeeFixture.NationalID = null, Throws.Exception.TypeOf<RequiredFieldException>());
        }
        [Test]
        public void NationalID_IsNotInitialized_ThrowsRequiredFieldException()
        {
            Expect(() => employeeFixture.NationalID = IRNationalID.Empty,
            Throws.Exception.TypeOf<RequiredFieldException>());
        }
        [Test, TestCase("123-456789-0"), TestCase("1234567890")]
        public void NationalID_IsInvalid_ThrowsInvalidNationalIDException(string invalidNationalID)
        {
            Expect(() => employeeFixture.NationalID = new IRNationalID(invalidNationalID),
            Throws.Exception.TypeOf<InvalidIRNationalIDException>());
        }
        [Test, TestCase("0000000043"), TestCase("000-000004-3")]
        public void NationalID_IsValid_Passes(string validNationalID)
        {
            Expect(() => employeeFixture.NationalID = new IRNationalID(validNationalID),
            Throws.Nothing);
        }
        #endregion NationalID
        #region IdentityNumber


        [TestCaseSource(typeof(CommonTestCaseSourceProvider), "NullOrEmptyOrWhitespace")]
        public void IdentityNumber_IsNullOrEmptyOrWhitespace_ThrowsRequiredFieldException(string invalidIdentityNumber)
        {
            Expect(() => employeeFixture.IdentityNumber = invalidIdentityNumber,
            Throws.Exception.TypeOf<RequiredFieldException>());
        }

        [Test, TestCase("abcd"), TestCase("1232#$!$"), TestCase("232 232")]
        public void IdentityNumber_ContainsIllegalCharacters_ThrowsInvalidIdentityNumberException(string invalidIdentityNumber)
        {
            Expect(() => employeeFixture.IdentityNumber = invalidIdentityNumber,
            Throws.Exception.TypeOf<InvalidIdentityNumberException>());
        }

        [Test, TestCase("12345"), TestCase("1")]
        public void IdentityNumber_IsValid_Passes(string validIdentityNumber)
        {
            Expect(() => employeeFixture.IdentityNumber = validIdentityNumber,
            Throws.Nothing);
        }
        #endregion
        #region EmployeeID
        [Test]
        public void EmployeeID_IsNotInitialized_ThrowsRequiredFieldException()
        {
            Expect(() => employeeFixture.EmployeeID = EmployeeID.Empty,
            Throws.Exception.TypeOf<RequiredFieldException>());
        }
        [Test, TestCase(01, 12, 01, 2001, 11, 01), TestCase(01, 12, 01, 2002, 12, 01)]
        public void EmployeeID_IsInvalid_ThrowsInvalidEmployeeIDException(int yearPart, int monthPart, int serialPart, int doeYear, int doeMonth, int doeDay)
        {

            employeeFixture.DateOfEmployement = new DateTime(doeYear, doeMonth, doeDay);
            Expect(() => employeeFixture.EmployeeID = new EmployeeID(yearPart, monthPart, serialPart),
            Throws.Exception.TypeOf<InvalidEmployeeIDException>());
        }
        [Test, TestCase(01, 12, 01, 2001, 12, 01), TestCase(01, 11, 01, 2001, 11, 01)]
        public void EmployeeID_WithValidInput_Passes(int yearPart, int monthPart, int serialPart, int doeYear, int doeMonth, int doeDay)
        {
            employeeFixture.DateOfEmployement = new DateTime(doeYear, doeMonth, doeDay);
            Expect(() => employeeFixture.EmployeeID = new EmployeeID(yearPart, monthPart, serialPart),
            Throws.Nothing);
        }

        #endregion
        #region ProfilePicture
        [Test]
        public void ProfilePicture_IsNull_ThrowsExceptionRequiredField()
        {
            Expect(() => employeeFixture.ProfilePicture = null, Throws.Exception.TypeOf<RequiredFieldException>());
        }
        //[Test]
        //public void ProfilePicture_SizeIsTooLarge_ThrowsInvalidImageSizeException()
        //{
        //    Expect(() => employeeFixture.ProfilePicture = new ProfilePicture(new Bitmap(400, 400)), Throws.Exception.TypeOf<InvalidImageSizeException>());
        //}
        #endregion
        #region ContactInformations
        [Test]
        public void ContactInformations_IsNotNull_Passes()
        {
            Expect(employeeFixture.ContactInformations, Is.Not.Null);
        }
        [Test]
        public void ContactInformations_HasNoContactInformation_Fails()
        {
            Expect(employeeFixture.ContactInformations.Count, Is.EqualTo(0));
        }
        #endregion
        #region Workshift
        [Test]
        public void WorkSchedule_IsNotNull_Passes()
        {
            Expect(employeeFixture.WorkSchedule, Is.Not.Null);
        }
        [Test]
        public void WorkSchedule_IsEqualToMinValue_Fails()
        {
            Expect(employeeFixture.WorkSchedule, Is.EqualTo(WorkSchedule.MinValue));
        }

        #endregion
        #region ID
        [Test]
        public void ID_IsSet_Passes()
        {
            Expect(() => employeeFixture.ID = Guid.NewGuid(), Is.Not.EqualTo(Guid.Empty));
        }
        [Test]
        public void ID_GetterSetter()
        {
            Guid guid = Guid.Parse("{66956516-13E3-46A9-8E51-F97C2E06E2BD}");
            employeeFixture.ID = guid;
            Expect(employeeFixture.ID, Is.EqualTo(Guid.Parse("{66956516-13E3-46A9-8E51-F97C2E06E2BD}")));
        }
        [Test]
        public void ID_IsEmpty_ThrowsRequiredFieldExceptionException()
        {
            Expect(() => employeeFixture.ID = Guid.Empty, Throws.Exception.TypeOf<RequiredFieldException>());
        }
        #endregion
        #region Clone
        [Test]
        public void Clone_ClonedEmployee_IsEqualToOriginalInstance()
        {
            Expect(() => employee_AhmadRahimi_0004000001.Clone(), Is.EqualTo(employee_AhmadRahimi_0004000001));
        }
        #endregion
        #region ToString
        [Test]
        public void ToString_WhenCalled_ReturnsFullNameAndEmployeeID()
        {
            Expect(employee_AhmadRahimi_0004000001.ToString(), Is.EqualTo("Ahmad Rahimi (0004000001)"));
        }
        #endregion
        #region Equals
        [Test]
        public void Equals_IsEqual_Passes()
        {
            Employee A = employee_AhmadRahimi_0004000001;
            Employee B = employee_AhmadRahimi_0004000001;
            Expect(A.Equals(B), Is.True);
        }
        [Test]
        public void Equals_IsNotEqual_Fails()
        {

            Employee A = employee_AhmadRahimi_0004000001;
            Employee B = new Employee();
            Expect(A.Equals(B), Is.False);
        }
        [Test]
        public void Equals_InputIsNull_Fails()
        {
            Employee A = new Employee();
            Expect(A.Equals(null), Is.False);
        }

        [Test]
        public void TrueGetHashCode()
        {
             Employee x = new Employee() { FirstName = "Abbas", ProfilePicture = new Bitmap(1, 1) };
            Employee y = new Employee() { FirstName = "Abbas", ProfilePicture = new Bitmap(1, 1) };
            Employee z = new Employee() { FirstName = "Abbas", ProfilePicture = new Bitmap(1, 1) };
  //          If two objects of the same type represent the same value, the hash function must return the same constant value for either object.
//For the best performance, a hash function must generate a random distribution for all input.
//The hash function must return exactly the same value regardless of any changes that are made to the object.
            Expect(()=>x.GetHashCode(),Is.EqualTo(y.GetHashCode()));
        }
        [Test]
        public void TrueEquals()
        {
            Employee x = new Employee() { FirstName = "Abbas", ProfilePicture = new Bitmap(1, 1) };
            Employee y = new Employee() { FirstName = "Abbas", ProfilePicture = new Bitmap(1, 1) };
            Employee z = new Employee() { FirstName = "Abbas", ProfilePicture = new Bitmap(1, 1) };
            //x.Equals(x) returns true.
            Expect(() => x.Equals(x), Is.True);

            //x.Equals(y) returns the same value as y.Equals(x).
            Expect(() => x.Equals(y), Is.EqualTo(y.Equals(x)));

            //if (x.Equals(y) && y.Equals(z)) returns true, then x.Equals(z) returns true.
            Expect(() => x.Equals(y) && y.Equals(z), Is.True);
            Expect(() => x.Equals(z), Is.True);


            // x.Equals(null) returns false.
            Expect(() => x.Equals(null), Is.False);
            //Successive invocations of x.Equals(y) return the same value as long as the objects referenced by x and y are not modified.
            /*
            x.Equals(x) returns true.
            x.Equals(y) returns the same value as y.Equals(x).
            if (x.Equals(y) && y.Equals(z)) returns true, then x.Equals(z) returns true.
            Successive invocations of x.Equals(y) return the same value as long as the objects referenced by x and y are not modified.
            x.Equals(null) returns false.
             */

        }
        #endregion
        #region == and != Operators
        [Test]
        public void EqualOperator_WithEqualInput_ReturnsTrue()
        {
            Employee A = employee_AhmadRahimi_0004000001;
            Employee B = employee_AhmadRahimi_0004000001;
            Expect(() => A == B, Is.True);
        }
        [Test]
        public void EqualOperator_WithUnequalInput_ReturnsFalse()
        {
            Employee A = employee_AhmadRahimi_0004000001;
            Employee B = new Employee();
            Expect(() => A == B, Is.False);
        }
        [Test]
        public void EqualOperator_WithNullInput_ReturnsFalse()
        {
            Employee A = null;
            Employee B = new Employee();
            Expect(() => A == B, Is.False);
        }
        [Test]
        public void UnequalOperator_WithNullInput_ReturnsTrue()
        {
            Employee A = null;
            Employee B = new Employee();
            Expect(() => A != B, Is.True);
        }
        [Test]
        public void UnequalOperator_WithUnequalInput_ReturnsTrue()
        {

            Employee A = employee_AhmadRahimi_0004000001;
            Employee B = new Employee();
            Expect(() => A != B, Is.True);
        }
        [Test]
        public void UnequalOperator_WithEqualInput_ReturnsFalse()
        {
            Employee A = employee_AhmadRahimi_0004000001;
            Employee B = employee_AhmadRahimi_0004000001;
            Expect(() => A != B, Is.False);
        }
        #endregion
        #region GetHashCode
        [Test]
        public void GetHashCode_WhenCalled_Passes()
        {
            Expect(employee_AhmadRahimi_0004000001.GetHashCode(), Is.EqualTo(employee_AhmadRahimi_0004000001.Clone().GetHashCode()));
        }
        #endregion


    }
}
