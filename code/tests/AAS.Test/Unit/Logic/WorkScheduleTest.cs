using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAS.Entities;
using AAS.Entities.Exceptions;
using NUnit.Framework;
using System.Diagnostics;
namespace AAS.Entities.Test.Unit
{
    public class WorkScheduleTest : AssertionHelper
    { 
        #region Test Data
        DayOfWeek[] DaysOfWeek = { 
                                     DayOfWeek.Saturday, 
                                     DayOfWeek.Sunday, 
                                     DayOfWeek.Monday, 
                                     DayOfWeek.Tuesday, 
                                     DayOfWeek.Wednesday, 
                                     DayOfWeek.Thursday, 
                                     DayOfWeek.Friday 
                                 };
        public string[] InvalidWorkshift = { 
            "0107780-0-0-0-0-0-0", 
            "3-0-0-0-0-0",
            "4-0-0-0-0-0-0-2", 
            "-0-0-0-0-0-0",
            "231",
            "",
            " " ,
            "gggg-0-0-0-0-0-0",
            "0000000"                                 
        };
        public static string[] ValidWorkshiftString = { 
           "7780-0-0-0-0-0-0", 
           "007780-000000-000000-000000-000000-000000-000000",
           "FFFFFF-0-0-0-0-0-0",
           "0-0-0-0-0-0-0",
           "ffffff-ffffff-ffffff-ffffff-ffffff-ffffff-ffffff"
        };
        public static WorkSchedule[] ValidWorkShift =
        {
            WorkSchedule.Parse(ValidWorkshiftString[0]),
            WorkSchedule.Parse(ValidWorkshiftString[0]),
            WorkSchedule.Parse(ValidWorkshiftString[2]),
            WorkSchedule.MinValue,
            WorkSchedule.MaxValue
        };
        List<TestCaseData> ValidWorkshiftReturnsSerializedString = new List<TestCaseData>()
        {
            new TestCaseData(ValidWorkShift[0]).Returns("7780-0-0-0-0-0-0"),
            new TestCaseData(ValidWorkShift[2]).Returns("ffffff-0-0-0-0-0-0"),
            new TestCaseData(WorkSchedule.MaxValue).Returns("ffffff-ffffff-ffffff-ffffff-ffffff-ffffff-ffffff"),
            new TestCaseData(WorkSchedule.MinValue).Returns("0-0-0-0-0-0-0"),
        };
        public List<TestCaseData> ValidWorkshiftReturnsValidOutPut = new List<TestCaseData>() {
            new TestCaseData("7780-0-0-0-0-0-0").Returns(ValidWorkShift[0]),
            new TestCaseData("007780-000000-000000-000000-000000-000000-000000").Returns(ValidWorkShift[0]),
            new TestCaseData("FFFFFF-0-0-0-0-0-0").Returns(ValidWorkShift[2]),
            new TestCaseData("ffffff-0-0-0-0-0-0").Returns(ValidWorkShift[2]),
            new TestCaseData("ffffff-ffffff-ffffff-ffffff-ffffff-ffffff-ffffff").Returns(WorkSchedule.MaxValue),
            new TestCaseData("0-0-0-0-0-0-0").Returns(WorkSchedule.MinValue)
        };

        WorkSchedule workScheduleFixture;
        static bool[] _9to5;

        #endregion
        #region SetUp
        [SetUp]
        public void SetUP()
        {
            workScheduleFixture = new WorkSchedule();

            _9to5 = new bool[24]{
                WorkSchedule.State.Off.ToBoolean(),WorkSchedule.State.Off.ToBoolean(),WorkSchedule.State.Off.ToBoolean(),WorkSchedule.State.Off.ToBoolean(),
                WorkSchedule.State.Off.ToBoolean(),WorkSchedule.State.Off.ToBoolean(),WorkSchedule.State.Off.ToBoolean(),WorkSchedule.State.Off.ToBoolean(),
                WorkSchedule.State.Off.ToBoolean(),WorkSchedule.State.Work.ToBoolean(),WorkSchedule.State.Work.ToBoolean(),WorkSchedule.State.Work.ToBoolean(),
                WorkSchedule.State.Off.ToBoolean(),WorkSchedule.State.Work.ToBoolean(),WorkSchedule.State.Work.ToBoolean(),WorkSchedule.State.Work.ToBoolean(),
                WorkSchedule.State.Work.ToBoolean(),WorkSchedule.State.Off.ToBoolean(),WorkSchedule.State.Off.ToBoolean(),WorkSchedule.State.Off.ToBoolean(),
                WorkSchedule.State.Off.ToBoolean(),WorkSchedule.State.Off.ToBoolean(),WorkSchedule.State.Off.ToBoolean(),WorkSchedule.State.Off.ToBoolean()
            };
        } 
        #endregion

        #region MaxValue & MinValue
        [Test]
        public void MaxValue_IsValid_Passes()
        {
            WorkSchedule parsedWorkshift = WorkSchedule.MaxValue;
            Expect(parsedWorkshift.ToString(), Is.EqualTo("ffffff-ffffff-ffffff-ffffff-ffffff-ffffff-ffffff"));
        }
        [Test]
        public void MinValue_IsValid_Passes()
        {
            WorkSchedule parsedWorkshift = WorkSchedule.MinValue;
            Expect(parsedWorkshift.ToString(), Is.EqualTo("0-0-0-0-0-0-0"));
        }
        #endregion
        #region DefineRange
        [Test]
        public void DefineRange_WithValidInput_ReturnsValidOutput()
        {

            workScheduleFixture.DefineRange(DayOfWeek.Saturday, 9, 12, WorkSchedule.State.Work);
            workScheduleFixture.DefineRange(DayOfWeek.Saturday, 12, 13, WorkSchedule.State.Off);
            workScheduleFixture.DefineRange(DayOfWeek.Saturday, 13, 17, WorkSchedule.State.Work);
            Expect(() => workScheduleFixture[DayOfWeek.Saturday], Is.EqualTo(_9to5));

        }
        [Test]
        public void DefineRange_WithLargeNumberAsBeginParam_ThrowsArgumentOutOfRangeException()
        {
            Expect(() => workScheduleFixture.DefineRange(DayOfWeek.Saturday, 26, 13, WorkSchedule.State.Off), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }
        [Test]
        public void DefineRange_WithSmallNumberAsBeginParam_ThrowsArgumentOutOfRangeException()
        {
            Expect(() => workScheduleFixture.DefineRange(DayOfWeek.Saturday, -2, 13, WorkSchedule.State.Off), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }
        [Test]
        public void DefineRange_WithLargeNumberAsEndnParam_ThrowsArgumentOutOfRangeException()
        {
            Expect(() => workScheduleFixture.DefineRange(DayOfWeek.Saturday, 5, 29, WorkSchedule.State.Off), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }
        [Test]
        public void DefineRange_WithSmallNumberAsEndParam_ThrowsArgumentOutOfRangeException()
        {
            Expect(() => workScheduleFixture.DefineRange(DayOfWeek.Saturday, 9, -23, WorkSchedule.State.Off), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }
        [Test]
        public void DefineRange_WithEndSmallerThanBegin_ThrowsInvalidInputEception()
        {
            Expect(() => workScheduleFixture.DefineRange(DayOfWeek.Saturday, 9, 3, WorkSchedule.State.Off), Throws.Exception.TypeOf<InvalidInputException>());
        }
        #endregion
        #region Define
        [Test]
        public void Define_AnHourIsDefined_ReturnExpectedValue()
        {
            workScheduleFixture.Define(DayOfWeek.Saturday, 3, WorkSchedule.State.Work);
            Expect(() => workScheduleFixture[DayOfWeek.Saturday][3], Is.EqualTo(WorkSchedule.State.Work.ToBoolean()));
        }
        [Test]
        public void Define_HourIsTooLarge_ThrowsArgumentOutOfRangeException()
        {
            Expect(() => workScheduleFixture.Define(DayOfWeek.Saturday, 26, WorkSchedule.State.Work), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }
        [Test]
        public void Define_HourIsTooSmall_ThrowsArgumentOutOfRangeException()
        {
            Expect(() => workScheduleFixture.Define(DayOfWeek.Saturday, -23, WorkSchedule.State.Work), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }
        #endregion
        #region Indexer
        [Test]
        public void Indexer_IndexIsTooLarge_ThrowsArgumentOutOfRangeException()
        {
            Expect(() => workScheduleFixture[DayOfWeek.Saturday][24], Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }
        [Test]
        public void Indexer_IndexIsTooSmall_ThrowsArgumentOutOfRangeException()
        {
            Expect(() => workScheduleFixture[DayOfWeek.Saturday][-1], Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        #endregion
        #region Parse
        [Test, TestCaseSource("ValidWorkshiftReturnsValidOutPut")]
        public WorkSchedule Parse_WithValidInput_ReturnsValidOutput(string validInput)
        {
            return WorkSchedule.Parse(validInput);
        }

        [Test, TestCaseSource("ValidWorkshiftString")]
        public void Parse_WithValidInput_Passes(string validInput)
        {
            Expect(() => WorkSchedule.Parse(validInput), Throws.Nothing);
        }

        [Test, TestCaseSource("InvalidWorkshift")]
        public void Parse_WithInValidInput_ThrowsInvalidFormatException(string invalidInput)
        {
            Expect(() => WorkSchedule.Parse(invalidInput), Throws.Exception.TypeOf<InvalidFormatException>());
        }
        [Test]
        public void Parse_WithNullInput_ThrowsArgumentNullExceptionException()
        {
            Expect(() => WorkSchedule.Parse(null), Throws.Exception.TypeOf<ArgumentNullException>());
        }

        #endregion
        #region ToString
        [Test, TestCaseSource("ValidWorkshiftReturnsSerializedString")]
        public string ToString_WithWorkshift_ReturnsString(WorkSchedule workshift)
        {
            return workshift.ToString();
        }
        #endregion
        #region Equals
        [Test]
        public void Equals_AIsEqualToB_ReturnsTrue()
        {
            WorkSchedule A = WorkSchedule.MaxValue;
            WorkSchedule B = WorkSchedule.MaxValue;
            Expect(() => A.Equals(B), Is.True);
        }
        [Test]
        public void Equals_AIsNotEqualToB_ReturnsFalse()
        {
            WorkSchedule A = WorkSchedule.MaxValue;
            WorkSchedule B = WorkSchedule.MinValue;
            Expect(() => A.Equals(B), Is.False);
        }
        [Test]
        public void Equals_BIsNull_ReturnsFalse()
        {
            WorkSchedule A = WorkSchedule.MaxValue;
            WorkSchedule B = null;
            Expect(() => A.Equals(B), Is.False);
        }
        #endregion
        #region GetHashCode
        [Test]
        public void GetHashCode_ForMaxValue_Passes()
        {
            Expect(() => WorkSchedule.MaxValue.GetHashCode(), Is.EqualTo(WorkSchedule.MaxValue.Clone().GetHashCode()));
        }
        #endregion
        #region Clone
        [Test]
        public void Clone_ClonedWorkSchedule_IsEqualToOriginalInstance()
        {
            Expect(() => workScheduleFixture.Clone(), Is.EqualTo(workScheduleFixture));
        }
        #endregion
        #region == and != Operators
        [Test]
        public void EqualOperator_WithEqualInput_ReturnsTrue()
        {
            WorkSchedule B = (WorkSchedule)workScheduleFixture.Clone();
            Expect(() => workScheduleFixture == B, Is.True);
        }
        [Test]
        public void EqualOperator_WithUnequalInput_ReturnsFalse()
        {
            WorkSchedule B = WorkSchedule.MaxValue;
            Expect(() => workScheduleFixture == B, Is.False);
        }
        [Test]
        public void EqualOperator_WithNullInput_ReturnsFalse()
        {
            Expect(() => workScheduleFixture == null, Is.False);
        }
        [Test]
        public void UnequalOperator_WithNullInput_ReturnsTrue()
        {
            Expect(() => workScheduleFixture != null, Is.True);
        }
        [Test]
        public void UnequalOperator_WithUnequalInput_ReturnsTrue()
        {

            WorkSchedule B = WorkSchedule.MaxValue;
            Expect(() => workScheduleFixture != B, Is.True);
        }
        [Test]
        public void UnequalOperator_WithEqualInput_ReturnsFalse()
        {

            WorkSchedule B = (WorkSchedule)workScheduleFixture.Clone();
            Expect(() => workScheduleFixture != B, Is.False);
        }
        #endregion

        #region GetBeginHour
        [Test, TestCaseSource("DaysOfWeek")]
        public void GetBeginHour_WhenCalled_ReturnsBeginHourInDay(DayOfWeek dayOfWeek)
        {
            workScheduleFixture.DefineRange(dayOfWeek, 9, 17, WorkSchedule.State.Work);
            Expect(() => workScheduleFixture.GetStartHour(dayOfWeek), Is.EqualTo(9));
        } 
        #endregion
        #region GetEndHour
        [Test, TestCaseSource("DaysOfWeek")]
        public void GetEndHour_WhenCalled_ReturnsEndHourInDay(DayOfWeek dayOfWeek)
        {
            workScheduleFixture.DefineRange(dayOfWeek, 9, 17, WorkSchedule.State.Work);
            Expect(() => workScheduleFixture.GetEndHour(dayOfWeek), Is.EqualTo(17));
        }
        #endregion
    }
}
