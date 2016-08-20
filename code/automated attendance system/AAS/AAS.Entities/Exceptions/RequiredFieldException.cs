using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AAS.Entities.Exceptions
{
    /// <summary>
    /// ورود فیلد الزامی است
    /// </summary>
    public class RequiredFieldException : AASExceptionBase
    {
        public string FieldName { get; set; }
        public RequiredFieldException([CallerMemberNameAttribute]string fieldName=""):base(fieldName)
        {
            FieldName = fieldName;
            Data.Add("FieldName", FieldName);
        }
    }
}
