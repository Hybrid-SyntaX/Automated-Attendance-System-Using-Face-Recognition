using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAS.Entities.Exceptions
{

    /// <summary>
    /// فرمت اشتباه است
    /// </summary>
    public class InvalidFormatException : AASExceptionBase
    {
        public InvalidFormatException():base("Invalid Format")
        {

        }
        public InvalidFormatException(string message):base(message)
        {

        }
    }
}
