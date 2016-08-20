using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAS.Entities.Exceptions
{
    /// <summary>
    /// ورودی نامعتبر
    /// </summary>
    public class InvalidInputException : AASExceptionBase
    {
        public InvalidInputException()
        {
                
        }
        public InvalidInputException(string message)
            : base(message)
        {

        }
    }
}
