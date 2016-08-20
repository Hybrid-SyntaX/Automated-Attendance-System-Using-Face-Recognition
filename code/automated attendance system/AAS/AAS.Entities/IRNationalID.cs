using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAS.Entities.Exceptions;
using AAS.Util;
using System.Runtime.Serialization;
using AAS.Entities.Resources;
namespace AAS.Entities
{
    /// <summary>
    /// کدملی
    /// </summary>
    [DataContract]
    public class IRNationalID : ICloneable
    {
        [DataMember]
        private string m_value;

        /// <summary>
        /// متد سازنده
        /// </summary>
        /// <exception cref="InvalidIRNationalIDFormatException" >فرمت کد ملی نا معتبر است</exception>
        /// <param name="irNationalIDString">رشته ورودی</param>
        public IRNationalID()
        {
            m_value = IRNationalID.Empty.m_value;
        }

        /// <summary>
        /// متد سازنده IRNationalID
        /// </summary>
        /// <exception cref="ArgumentNullException" >ورودی خالی داده شده</exception>
        /// <exception cref="InvalidIRNationalIDFormatException" >فرمت کد ملی نا معتبر است</exception>
        /// <param name="irNationalIDString">رشته کد ملی</param>
        public IRNationalID(string irNationalIDString)
        {

            value = irNationalIDString;

        }

        /// <summary>
        /// با استفاده از رشته ورودی یکه IRNationalID می سازد
        /// </summary>
        /// <param name="s">رشته متناظر با IRNAtionalID</param>
        /// <returns>IRNationalID</returns>
        public static IRNationalID Parse(string s)
        {
            try
            {

                return new IRNationalID(s);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// با استفاده از رشته ورودی یک IRNationalID می سازد. و موفقیت یا عدم موفقیت عمل را بر می گرداند.
        /// </summary>
        /// <param name="s">رشته متناظر با IRNAtionalID</param>
        /// <param name="result">IRNationalID ایجاد شده</param>
        /// <returns>نتیجه عمل Parse</returns>
        public static bool TryParse(string s, out IRNationalID result)
        {
            result = null;
            try
            {
                result = IRNationalID.Parse(s);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// کد ملی را نگه داری می کند
        /// </summary>
        /// <exception cref="ArgumentNullException" >ورودی خالی داده شده</exception>
        /// <exception cref="InvalidIRNationalIDFormatException" >فرمت کد ملی نا معتبر است</exception>
        protected virtual string value
        {
            set
            {
                if (Validator.IsNullOrEmptyOrWhitespace(value))
                    throw new ArgumentNullException("s");

                if (!Validator.IsMatch(value, RegExPatterns.IRNationalID))
                    throw new InvalidIRNationalIDFormatException();
                m_value = stripDashes(value);
            }
            get
            {
                return m_value;
            }

        }


        /// <summary>
        /// یک IRNationalID خالی
        /// </summary>
        public static IRNationalID Empty
        {
            get
            {

                return new IRNationalID("000-000000-0");
            }

        }

        /// <summary>
        /// یک IRNationalID نامعتبر
        /// </summary>
        public static IRNationalID InvalidValue
        {
            get
            {
                return new IRNationalID("999-999999-9");
            }
        }

        /// <summary>
        /// صحت کد ملی وارد شده را برسی می کند @cite fa_wiki_irnationalid
        /// </summary>
        /// <param name="input">رشته کد ملی</param>
        /// <returns>bool</returns>
        public static bool IsValid(IRNationalID value)
        {
            string input = value.m_value;

            float sum = 0;
            int controlDigit = int.Parse(input[input.Length - 1].ToString());
            ///[Multilplying each digit by its own position and summing them up]
            for (int i = 0; i < 9; i++)
            {
                sum += float.Parse(input[i].ToString()) * (10 - i);
            }
            ///[Multilplying each digit by its own position and summing them up]
            
            float mod = sum % 11;
            ///[Either less than two or equal to control digit]
            if (mod < 2 && mod == controlDigit)
                return true;
            else if (11 - mod == controlDigit)
                return true;
            ///[Either less than two or equal to control digit]

            return false;
        }

        /// <summary>
        /// - را از ورودی حذف می کند
        /// </summary>
        /// <param name="input">رشته ورودی</param>
        /// <returns>string</returns>
        private static string stripDashes(string input)
        {
            if (input[3] == '-' && input[10] == '-')
                input = input.Remove(3, 1).Remove(9, 1);
            return input;
        }

        /// <summary>
        /// قسمت های مختلف کد ملی را با - جدا کرده، و رشته جدید را بر می گرداند.
        /// </summary>
        /// <param name="input">رشته ورودی</param>
        /// <returns>string</returns>
        private static string putDashes(string input)
        {
            if (input == null)
                return Empty.ToString();

            if (input.Contains('-'))
                return input;

            return input.Insert(3, "-").Insert(10, "-");
        }


        /// <summary>
        /// برسی یکسان بودن نمونه جاری با نمونه ورودی
        /// </summary>
        /// <param name="obj">نمونه وردی</param>
        /// <returns>نتیجه برسی</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is IRNationalID))
                return false;

            IRNationalID a = this;
            IRNationalID b = (IRNationalID)obj;
            return a.m_value == b.m_value;
        }
        
        /// <summary>
        /// مقدار HashCode نمونه جاری
        /// </summary>
        /// <returns>مقدار HashCode</returns>
        public override int GetHashCode()
        {
            return m_value.GetHashCode();
        }

        /// <summary>
        /// عملگر ==
        /// </summary>
        /// <param name="a">نمونه اول</param>
        /// <param name="b">نمونه دوم</param>
        /// <returns>نتیجه برسی</returns>
        /// <seealso cref="!="/>
        public static bool operator ==(IRNationalID a, IRNationalID b)
        {
            if (object.ReferenceEquals(a, b))
                return true;

            if ((object)a == null || (object)b == null)
                return false;

            return a.Equals(b);
        }

        /// <summary>
        /// عملگر !=
        /// </summary>
        /// <param name="a">نمونه اول</param>
        /// <param name="b">نمونه دوم</param>
        /// <returns>نتیجه برسی</returns>
        ///<seealso cref="=="/>
        public static bool operator !=(IRNationalID a, IRNationalID b)
        {
            return !(a == b);
        }
 
        /// <summary>
        /// ایجاد یک کپی کامل از نمونه جاری
        /// </summary>
        /// <returns>نمونه کپی شده</returns>
        public object Clone()
        {
            return new IRNationalID(m_value);
        }

        /// <summary>
        /// نمایش رشته ای IRNationalID
        /// </summary>
        /// <returns>رشته متناظر با IRNationalID</returns>
        /// <seealso cref="putDashes"/>
        public override string ToString()
        {
            return putDashes(m_value);
        }
    }
}
