using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using AAS.Service;
using System.ServiceModel.Description;
namespace AAS.Server
{
    /// <summary>
    /// برنامه سرور سیستم خودکار حضور و غیاب
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
              
                ServiceHost host = new ServiceHost(typeof(AAS.Service.AutomatedAttendanceSystem));
                host.Open();
                if (host.State == CommunicationState.Opened)
                {

                    Console.WriteLine("AAS Service is host at " + DateTime.Now.ToString());
                    Console.WriteLine("Host is running... Press <Enter> key to stop");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Service couldn't run");
                }
            }
            catch (Exception ex)
            {

                throw new FaultException(string.Format("An execption occured in the server\n\n{0}",ex.Message));
            }
        }
    }
}
