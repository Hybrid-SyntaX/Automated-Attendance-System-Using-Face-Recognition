using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAS.Entities.Exceptions
{
    /// <summary>
    /// تاریخ نا معتبر
    /// </summary>
    public class InvalidDateException : AASExceptionBase
    {
        public InvalidDateException()
        {

        }
        public InvalidDateException(string description):base(description)
        {

        }
    }
}
