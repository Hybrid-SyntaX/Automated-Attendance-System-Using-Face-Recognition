using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using AAS.Service;
using AAS.Entities.Test;
using AAS.Proxy;
namespace AAS.Test.Unit.Proxy
{
    public class AutomatedAttendanceSystemProxyTest:AssertionHelper
    {
#if false
        System.Diagnostics.Process AASServer;
        public AutomatedAttendanceSystemProxyTest()
        {
            AASServer = System.Diagnostics.Process.Start(@"G:\Codes\Desktop\Automated Attendance System Using Facial Recognitin\codes\AAS\AAS.Server\bin\Debug\AAS.Server.exe");
            
        }
        ~AutomatedAttendanceSystemProxyTest()
        {
            AASServer.Close();
            AASServer.CloseMainWindow();
            AASServer.WaitForExit();
            
        }
#endif

        IAutomatedAttendanceSystem AutomatedAttendanceSystem;
        [SetUp]
        public void SetUp()
        {

            //this.AutomatedAttendanceSystem = new AutomatedAttendanceSystemProxy();

            AutomatedAttendanceSystem=MockRepository.GenerateDynamicMockWithRemoting<IAutomatedAttendanceSystem>();
            
            AutomatedAttendanceSystem.Initialize();
            AutomatedAttendanceSystem.CreateEmployee(CommonTestCaseSourceProvider.newEmployeeSample());
           // AutomatedAttendanceSystem.CreateEmployee(CommonTestCaseSourceProvider.newEmployeeSample());
        }
        [Test]
        public void AutomatedAttendanceSystemIsMade()
        {
            Expect(() => AutomatedAttendanceSystem, Is.Not.Null);
        }
        [Test]
        public void RetrieveEmployees()
        {
            Expect(() => AutomatedAttendanceSystem.RetrieveEmployees(), Count.GreaterThan(0));
        }
    }
}
