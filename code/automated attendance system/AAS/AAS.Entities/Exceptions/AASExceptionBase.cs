using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
namespace AAS.Entities.Exceptions
{
    /// <summary>
    /// کلاس پایه برای تمام Exception های موجود در سیستم
    /// </summary>
    public class AASExceptionBase : FaultException
    {
        public AASExceptionBase():base("AAS Exception")
        {
        }
        public AASExceptionBase(string message)
            : base(message)
        {

        }
    }
}
