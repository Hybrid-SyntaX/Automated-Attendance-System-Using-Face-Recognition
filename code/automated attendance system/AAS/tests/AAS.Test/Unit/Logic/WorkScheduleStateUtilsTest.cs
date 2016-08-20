using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AAS.Entities.Test.Unit
{
    public class WorkScheduleStateUtilsTest:AssertionHelper
    {

        TestCaseData[] StateReturnsBoolean = new TestCaseData[]
        { 
            new TestCaseData(WorkSchedule.State.Off).Returns(false),
            new TestCaseData(WorkSchedule.State.Work).Returns(true)
        };

        [Test,TestCaseSource("StateReturnsBoolean")]
        public bool ToBoolean_WithValidInput_ReturnsExpectedOutput(WorkSchedule.State state)
        {
            return state.ToBoolean();
        }
        [Test]
        public void ToBoolean_WithInvalidState_ThrowsArgumentOutOfRangeException()
        {
            Expect(()=>((WorkSchedule.State)(3)).ToBoolean(), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }
        
    }
}
