using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AAS.Entities;
using AAS.Entities.Exceptions;
using AAS.Util;
namespace AAS.Entities.Test.Unit
{
    public class EmployeeIDTest : AssertionHelper
    {
        #region Test Data
        EmployeeID employeeIDFixture0603000001;
        List<TestCaseData> validEmployeeIDs = new List<TestCaseData>() 
        { 
            new TestCaseData("0612000031", 06, 12, 31)
            ,new  TestCaseData("0612100001", 06, 12, 100001)
            ,new TestCaseData("0602000030",06,02,30)
        };
        #endregion
        #region SetUp
        [SetUp]
        public void SetUp()
        {
            employeeIDFixture0603000001 = new EmployeeID(06, 03, 01);
        }
        #endregion
        #region Equals
        [Test]
        public void Equals_IsEqual_Passes()
        {
            EmployeeID employeeId1 = new EmployeeID(06, 12, 31);
            EmployeeID employeeId2 = new EmployeeID(06, 12, 31);
            Expect(employeeId1.Equals(employeeId2), Is.True);
        }
        [Test]
        public void Equals_IsNotEqual_Fails()
        {
            EmployeeID employeeId1 = new EmployeeID(06, 12, 31);
            EmployeeID employeeId2 = new EmployeeID(06, 12, 12);
            Expect(employeeId1.Equals(employeeId2), Is.False);
        }
        [Test]
        public void Equals_InputIsNull_Fails()
        {
            EmployeeID employeeId1 = new EmployeeID(06, 12, 31);

            Expect(employeeId1.Equals(null), Is.False);
        }
        #endregion
        #region Parse
        [Test, TestCaseSource("validEmployeeIDs")]
        public void Parse_ProvidedWithValidInput_ReturnsEqualEmployeeId(string validInput, int yearPart, int monthPart, int serialPart)
        {
            EmployeeID employeeId = new EmployeeID(yearPart, monthPart, serialPart);
            Expect(() => EmployeeID.Parse(validInput)
            , Is.EqualTo(employeeId));
        }
        [Test, TestCase("061200003"), TestCase("0613000034"), TestCase("01121333312"), TestCase("ada")]
        public void Parse_ProvidedWithInvalidInput_ThrowsInvalidEmployeeIDFormatException(string invalidInput)
        {
            Expect(() => EmployeeID.Parse(invalidInput), Throws.Exception.TypeOf<InvalidEmployeeIDFormatException>());
        }
        #endregion Parse
        #region ToString
        [Test]
        public void ToStringFor0603000001Returns0603000001String()
        {
            Expect(employeeIDFixture0603000001.ToString(), Is.EqualTo("0603000001"));
        }
        #endregion
        #region GetHashCode
        [Test]
        public void GetHashCode_For0603000001_Passes()
        {
            Expect(employeeIDFixture0603000001.GetHashCode(), Is.EqualTo(employeeIDFixture0603000001.Clone().GetHashCode()));
        }
        #endregion
        #region IsValid
        [Test, TestCase("1312000023", 13, 12, 01)]
        public void IsValid_WithValidInput_Passes(string validEmployeeIdString, int year, int month, int day)
        {
            EmployeeID employeeId = EmployeeID.Parse(validEmployeeIdString);
            DateTime dateTime = new DateTime(year, month, day);
            Expect(EmployeeID.IsValid(employeeId, dateTime), Is.True);
        }
        [Test, TestCase("1312000023", 13, 11, 01), TestCase("1211000023", 13, 11, 01)]
        public void IsValid_WithInValidInput_Fails(string validEmployeeIdString, int year, int month, int day)
        {
            //TODO: needs more work
            EmployeeID employeeId = EmployeeID.Parse(validEmployeeIdString);
            DateTime dateTime = new DateTime(year, month, day);
            Expect(EmployeeID.IsValid(employeeId, dateTime), Is.False);
        }
        #endregion
        #region EmployeeID (Constructor)
        [Test]
        public void EmloyeeID_IsConstructedWithValidInput_Passes()
        {
            Expect(() => { EmployeeID employeeId = new EmployeeID(06, 10, 10); }, Throws.Nothing);
        }
        [Test, TestCase(06, 13, 10), TestCase(231, 11, 10), TestCase(21, 4, 1000002)]
        public void Employee_IsConstructedWithInvalidInput_ThrowsArgumentOutOfRangeException(int yearPart, int monthPart, int serialPart)
        {
            Expect(() => { EmployeeID employeeId = new EmployeeID(yearPart, monthPart, serialPart); }, Throws.Exception.TypeOf<ArgumentOutOfRangeException>());

        }
        #endregion
        #region Empty Field
        [Test]
        public void TestEmpty()
        {

            Expect(EmployeeID.Empty, Is.EqualTo(new EmployeeID(0, 0, 0)));
        }
        #endregion

        #region Clone
        [Test]
        public void Clone_ClonedEmployeeID_IsEqual()
        {
            Expect(() => employeeIDFixture0603000001.Clone(), Is.EqualTo(employeeIDFixture0603000001));
        }
        #endregion

        #region == and != Operators
        [Test]
        public void EqualOperator_WithEqualInput_ReturnsTrue()
        {
            EmployeeID B = new EmployeeID(6, 3, 1);
            Expect(() => employeeIDFixture0603000001 == B, Is.True);
        }
        [Test]
        public void EqualOperator_WithUnequalInput_ReturnsFalse()
        {
            EmployeeID B = new EmployeeID(6, 3, 2);
            Expect(() => employeeIDFixture0603000001 == B, Is.False);
        }
        [Test]
        public void EqualOperator_WithNullInput_ReturnsFalse()
        {
            Expect(() => employeeIDFixture0603000001 == null, Is.False);
        }
        [Test]
        public void UnequalOperator_WithNullInput_ReturnsTrue()
        {
            Expect(() => employeeIDFixture0603000001 != null, Is.True);
        }
        [Test]
        public void UnequalOperator_WithUnequalInput_ReturnsTrue()
        {
            EmployeeID B = new EmployeeID(6, 3, 2);
            Expect(() => employeeIDFixture0603000001 != B, Is.True);
        }
        [Test]
        public void UnequalOperator_WithEqualInput_ReturnsFalse()
        {
            EmployeeID B = new EmployeeID(6, 3, 1);
            Expect(() => employeeIDFixture0603000001 != B, Is.False);
        }
        #endregion

        #region NewEmployeeID
        [Test]
        public void NewEmployeeID_WhenCalledInTheSameYearAndMonth_IcrementsSerialPartAndGeneratesAndReturnsANewEmployeeID()
        {
            int l_yearPart = int.Parse(DateTime.Now.ToString("yy"));
            int l_monthPart = DateTime.Now.Month;
            List<Employee> employees = new List<Employee>()
            {
                new Employee()
                {
                    DateOfBirth=new DateTime(DateTime.Now.AddYears(-20).Year,01,01),
                    DateOfEmployement=new DateTime(DateTime.Now.Year,l_monthPart,01),
                    EmployeeID=new EmployeeID(l_yearPart, l_monthPart, 01)
                },
               new Employee()
                {
                    DateOfBirth=new DateTime(DateTime.Now.AddYears(-20).Year,01,01),
                    DateOfEmployement=new DateTime(DateTime.Now.Year,l_monthPart,01),
                    EmployeeID=new EmployeeID(l_yearPart, l_monthPart, 02)
                }

                
            };
            EmployeeID newEmployeeID = new EmployeeID(l_yearPart, l_monthPart, 03);
            Expect(() => EmployeeID.NewEmployeeID(employees), Is.EqualTo(newEmployeeID));
        }
        [Test]
        public void NewEmployeeID_WhenCalledInTheSameYearButDifferentMonth_StartsFromZeroAndSerialPartAndGeneratesAndReturnsANewEmployeeID()
        {
            int l_yearPart = int.Parse(DateTime.Now.ToString("yy"));
            int l_monthPart = 02;
            List<Employee> employees = new List<Employee>()
            {
                new Employee()
                {
                    DateOfBirth=new DateTime(DateTime.Now.AddYears(-20).Year,01,01),
                    DateOfEmployement=new DateTime(DateTime.Now.Year,l_monthPart,01),
                    EmployeeID=new EmployeeID(l_yearPart, l_monthPart, 01)
                },
               new Employee()
                {
                    DateOfBirth=new DateTime(DateTime.Now.AddYears(-20).Year,01,01),
                    DateOfEmployement=new DateTime(DateTime.Now.Year,l_monthPart,01),
                    EmployeeID=new EmployeeID(l_yearPart, l_monthPart, 02)
                }

                
            };
            EmployeeID newEmployeeID = new EmployeeID(l_yearPart, DateTime.Now.Month, 01);
            Expect(() => EmployeeID.NewEmployeeID(employees), Is.EqualTo(newEmployeeID));
        }
        [Test]
        public void NewEmployeeID_WhenCalledInTheDifferentYearAndMonth_StartsFromZeroAndSerialPartAndGeneratesAndReturnsANewEmployeeID()
        {
            int l_yearPart = 01;
            int l_monthPart = 02;
            List<Employee> employees = new List<Employee>()
            {
                new Employee()
                {
                    DateOfBirth=new DateTime(DateTime.Now.AddYears(-20).Year,01,01),
                    DateOfEmployement=new DateTime(2001,l_monthPart,01),
                    EmployeeID=new EmployeeID(l_yearPart, l_monthPart, 01)
                },
               new Employee()
                {
                    DateOfBirth=new DateTime(DateTime.Now.AddYears(-20).Year,01,01),
                    DateOfEmployement=new DateTime(2001,l_monthPart,01),
                    EmployeeID=new EmployeeID(l_yearPart, l_monthPart, 02)
                }

                
            };
            EmployeeID newEmployeeID = new EmployeeID(int.Parse(DateTime.Now.ToString("yy")), DateTime.Now.Month, 01);
            Expect(() => EmployeeID.NewEmployeeID(employees), Is.EqualTo(newEmployeeID));
        }
        [Test]
        public void NewEmployeeID_WhenCalledInTheDifferentYearButSameMonth_StartsFromZeroAndSerialPartAndGeneratesAndReturnsANewEmployeeID()
        {
            int l_yearPart = 01;
            int l_monthPart = DateTime.Now.Month;
            List<Employee> employees = new List<Employee>()
            {
                new Employee()
                {
                    DateOfBirth=new DateTime(DateTime.Now.AddYears(-20).Year,01,01),
                    DateOfEmployement=new DateTime(2001,l_monthPart,01),
                    EmployeeID=new EmployeeID(l_yearPart, l_monthPart, 01)
                },
               new Employee()
                {
                    DateOfBirth=new DateTime(DateTime.Now.AddYears(-20).Year,01,01),
                    DateOfEmployement=new DateTime(2001,l_monthPart,01),
                    EmployeeID=new EmployeeID(l_yearPart, l_monthPart, 02)
                }

                
            };
            EmployeeID newEmployeeID = new EmployeeID(int.Parse(DateTime.Now.ToString("yy")), l_monthPart, 01);
            Expect(() => EmployeeID.NewEmployeeID(employees), Is.EqualTo(newEmployeeID));
        }
        #endregion
    }
}
