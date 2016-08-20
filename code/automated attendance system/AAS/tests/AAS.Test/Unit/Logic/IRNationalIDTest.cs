using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AAS.Entities.Exceptions;
using AAS.Entities;
namespace AAS.Entities.Test.Unit
{
    public class IRNationalIDTest : AssertionHelper
    {
        #region TestData
        static string[] ValidIRNationalIDStrings = { "123-456789-0", "1234567890","0014567890","001-456789-0","14567890" };
        List<TestCaseData> ValidIRNationalID = new List<TestCaseData>()
        {
            new TestCaseData(ValidIRNationalIDStrings[0]).Returns(new IRNationalID(ValidIRNationalIDStrings[0])),
            new TestCaseData(ValidIRNationalIDStrings[1]).Returns(new IRNationalID(ValidIRNationalIDStrings[1])),
            new TestCaseData(ValidIRNationalIDStrings[1]).Returns(new IRNationalID(ValidIRNationalIDStrings[0])),
            new TestCaseData(ValidIRNationalIDStrings[0]).Returns(new IRNationalID(ValidIRNationalIDStrings[1]))
        };
        IRNationalID irNationalID0000000043;
        #endregion

        #region SetUp
        [SetUp]
        public void SetUp()
        {
            irNationalID0000000043 = new IRNationalID("000-000004-3");
        } 
        #endregion

        #region GetHashCode
        [Test]
        public void GetHashCode_For0000000043_Passes()
        {
            Expect(() => irNationalID0000000043.GetHashCode(), Is.EqualTo(irNationalID0000000043.Clone().GetHashCode()));
        }
        #endregion
     
        #region  Equals
        [Test]
        public void Equals_InputIsNull_ReturnsFalse()
        {
            Expect(() => irNationalID0000000043.Equals(null), Is.False);
        }
        [Test,TestCase("123-456789-0"),TestCase("1234567890")]
        public void Equals_InputIsNotEqual_ReturnsFalse(string unequalNationalIDString)
        {
            IRNationalID UnequalNationalId = new IRNationalID(unequalNationalIDString);
            Expect(() => irNationalID0000000043.Equals(UnequalNationalId), Is.False);
        }
        [Test, TestCase("0000000043"), TestCase("000-000004-3")]
        public void Equals_InputIsEqual_ReturnsTrue(string unequalNationalIDString)
        {
            IRNationalID UnequalNationalId = new IRNationalID(unequalNationalIDString);
            Expect(() => irNationalID0000000043.Equals(UnequalNationalId), Is.True);
        }
        #endregion
     
        #region IRNationalID (Constructor)
        [Test, TestCaseSource(typeof(CommonTestCaseSourceProvider), "NullOrEmptyOrWhitespace")]
        public void IRNationalID_WithNullOrEmptyOrWhitespaceInput_ThrowsArgumentNullException(string invalidInput)
        {
            Expect(() => new IRNationalID(invalidInput), Throws.Exception.TypeOf<ArgumentNullException>());
        }
        [Test, TestCase("2345678"), TestCase("12345678901"), TestCase("123-45678-90"), TestCase("12345678901")]
        public void IRNationalID_InputFormatIsInvalid_ThrowsInvalidIRNationalIDFormatExceptionn(string invalidInput)
        {
            Expect(() => new IRNationalID(invalidInput), Throws.Exception.TypeOf<InvalidIRNationalIDFormatException>());
        }
        [Test, TestCaseSource("ValidIRNationalIDStrings")]
        public void IRNationalID_InputIsValid_Passes(string validNationalID)
        {
            Expect(()=>new IRNationalID(validNationalID),Throws.Nothing);
        }
        [Test]
        public void IRNationalID_WhenParameterlessConstructorIsCalled_InitalizesWithEmptyIRNationalID()
        {
            Expect(() => new IRNationalID(), Is.EqualTo(IRNationalID.Empty));
        }
        #endregion
     
        #region IsValid
        [Test, TestCase("123-456789-0")]
        public void IsValid_NationalIDIsInvalid_Fails(string invalidNationalId)
        {
            Expect(() => IRNationalID.IsValid(new IRNationalID(invalidNationalId)), Is.False);
        }
        [Test,TestCase("0000000043"), TestCase("000-000004-3")]
        public void IsValid_NationalIDIsValid_Passes(string validNationalId)
        {
            Expect(() => IRNationalID.IsValid(new IRNationalID(validNationalId)), Is.True);
        }
        #endregion
    
        #region Empty Field

        [Test]
        public void Empty_Returns_IRNationalID0000000000()
        {
            IRNationalID emptyNationalID=new IRNationalID("000-000000-0");
            Expect(() => IRNationalID.Empty, Is.EqualTo(emptyNationalID));

        }
        #endregion

        #region == and != Operators
        [Test]
        public void EqualOperator_WithEqualInput_ReturnsTrue()
        {
            IRNationalID B = new IRNationalID("000-000004-3");
            Expect(() => irNationalID0000000043 == B, Is.True);
        }
        [Test]
        public void EqualOperator_WithUnequalInput_ReturnsFalse()
        {
            IRNationalID B = new IRNationalID("000-000003-5");
            Expect(() => irNationalID0000000043 == B, Is.False);

        }
        [Test]
        public void EqualOperator_WithNullInput_ReturnsFalse()
        {
            Expect(() => irNationalID0000000043 == null, Is.False);
        }
        [Test]
        public void UnequalOperator_WithNullInput_ReturnsTrue()
        {
            Expect(() => irNationalID0000000043 != null, Is.True);
        }   
        [Test]
        public void UnequalOperator_WithUnequalInput_ReturnsTrue()
        {
            IRNationalID B = new IRNationalID("000-000003-5");
            Expect(() => irNationalID0000000043 != B, Is.True);
        }
        [Test]
        public void UnequalOperator_WithEqualInput_ReturnsFalse()
        {
            IRNationalID B = new IRNationalID("000-000004-3");
            Expect(() => irNationalID0000000043 != B, Is.False);
        }
        #endregion

        #region ToString
        [Test]
        public void ToString_WhenCalled_ReturnsValidOutput()
        {
            Expect(() => irNationalID0000000043.ToString(), Is.EqualTo("000-000004-3"));
        }
        #endregion

        #region Clone
        [Test]
        public void Clone_ClonedIRNationalID_IsEqualToOriginalInstance()
        {
            Expect(() => irNationalID0000000043.Clone(), Is.EqualTo(irNationalID0000000043));
        }
        #endregion

        #region Parse
        [Test, TestCaseSource("ValidIRNationalID")]
        public IRNationalID Parse_WithValidInput_ReturnsIRNationalIDInstance(string validNationalID)
        {
            return IRNationalID.Parse(validNationalID);
        }
        #endregion
    }

}
