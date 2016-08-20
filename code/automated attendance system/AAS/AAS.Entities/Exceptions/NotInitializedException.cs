using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAS.Entities.Exceptions
{
    /// <summary>
    /// مقدار دهی اولیه نشده است
    /// </summary>
    public class NotInitializedException : AASExceptionBase
    {
        public NotInitializedException(string message):base(message)
        {

        }
    }
}
