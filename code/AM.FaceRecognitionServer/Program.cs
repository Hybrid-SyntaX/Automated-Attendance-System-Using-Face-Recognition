using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
namespace AM.FaceRecognitionServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                ServiceHost host = new ServiceHost(typeof(AM.FRS.FaceRecognitionService));
                host.Open();
                if (host.State == CommunicationState.Opened)
                {

                    Console.WriteLine("Face Recognition Service is host at " + DateTime.Now.ToString());
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

                throw new FaultException(string.Format("An execption occured in the server\n\n{0}", ex.Message));
            }
        }
    }
}
