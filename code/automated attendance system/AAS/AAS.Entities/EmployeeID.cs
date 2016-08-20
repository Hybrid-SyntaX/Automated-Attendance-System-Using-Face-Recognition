using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using AAS.Entities.Exceptions;
using System.Runtime.Serialization;
using System.Globalization;
namespace AAS.Entities
{
    /// <summary>
    /// شماره کارمندی
    /// </summary>
    [DataContract]
    public class EmployeeID : ICloneable
    {

        /// <summary>
        /// قسمت سال استخدام
        /// </summary>
        [DataMember]
        private int m_yearPart;
        /// <summary>
        /// قسمت ماه استخدام
        /// </summary>
        [DataMember]
        private int m_monthPart;
        /// <summary>
        /// قسمت سریال
        /// </summary>
        [DataMember]
        private int m_serialPart;

        /// <summary>
        /// یک EmployeeID خالی
        /// </summary>
        public static readonly EmployeeID Empty = new EmployeeID(0, 0, 0);

        /// <summary>
        /// ایجاد شماره کارمندی جدید
        /// </summary>
        /// <param name="employees">لیستی از تمام کارمندان</param>
        /// <returns>شماره کارمندی جدید</returns>
        public static EmployeeID NewEmployeeID(IList<Employee> employees)
        {
            ///[Initializing new employee id variables]
            GregorianCalendar greogianCalendar = new GregorianCalendar();


            int l_yearPart = int.Parse(greogianCalendar.GetYear(DateTime.Now).ToString().Remove(0, 2));
            int l_monthPart = greogianCalendar.GetMonth(DateTime.Now);
            int l_lastSerial = 0;
            ///[Initializing new employee id variables]


            ///[Determine l_lastSerial value]
            if (employees.Count > 0)
            {
                EmployeeID lastEmployeeID = employees.Last().EmployeeID;

                if (lastEmployeeID.m_monthPart == l_monthPart && lastEmployeeID.m_yearPart == l_yearPart)
                    l_lastSerial = lastEmployeeID.m_serialPart;
            }
            ///[Determine l_lastSerial value]

            ///[New employeeID is generated and returned]
            EmployeeID newEmployeeID = new EmployeeID();
            newEmployeeID.m_yearPart = int.Parse(greogianCalendar.GetYear(DateTime.Now).ToString().Remove(0, 2));
            newEmployeeID.m_monthPart = greogianCalendar.GetMonth(DateTime.Now);
            newEmployeeID.m_serialPart = l_lastSerial + 1;

            return newEmployeeID;
            ///[New employeeID is generated and returned]

        }

        /// <summary>
        /// متد سازنده
        /// </summary>
        public EmployeeID()
        {

        }

        /// <summary>
        /// متد سازنده EmployeeID
        /// </summary>
        /// <param name="yearPart">قسمت مربوط به سال استخدام در شماره کارمندی</param>
        /// <param name="monthPart">قسمت مربوط به ماه استخدام در شماره کارمندی</param>
        /// <param name="serialPart">قسمت مربوط به سریال در شماره کارمندی</param>
        /// <exception cref="ArgumentOutOfRangeException" />
        public EmployeeID(int yearPart, int monthPart, int serialPart)
        {
            if (!isMonthPartInValidRange(monthPart))
                throw new ArgumentOutOfRangeException("monthPart", monthPart, "monthPart part should be between 1 and 12");
            if (!isYearPartInValidRange(yearPart))
                throw new ArgumentOutOfRangeException("yearPart", yearPart, "yearPart should be between 0 and 99");
            if (!isSerialPartInValidRange(serialPart))
                throw new ArgumentOutOfRangeException("serialPart", serialPart, "serialPart should be between 0 and 99");

            m_yearPart = yearPart;
            m_monthPart = monthPart;
            m_serialPart = serialPart;
        }

        /// <summary>
        /// برسی محدوده عدد ورودی در قسمت سریال.
        /// این عدد باید بین 0 تا 999999 باشد
        /// </summary>
        /// <param name="serialPart">قسمت مربوط به سریال در شماره کارمندی</param>
        /// <returns>نتیجه برسی</returns>
        private static bool isSerialPartInValidRange(int serialPart)
        {
            return serialPart >= 0 && serialPart <= 999999;
        }

        /// <summary>
        /// برسی محدوده عدد ورودی در قسمت سال استخدام.
        /// این عدد باید بین 0 تا 99 باشد
        /// </summary>
        /// <param name="yearPart">قسمت مربوط به سال استخدام در شماره کارمندی</param>
        /// <returns>نتیجه برسی</returns>
        private static bool isYearPartInValidRange(int yearPart)
        {
            return yearPart >= 00 && yearPart <= 99;
        }

        /// <summary>
        /// برسی محدوده عدد ورودی در قسمت ماه استخدام.
        /// این عدد باید بین 0 تا 12 باشد
        /// </summary>
        /// <param name="monthPart">قسمت مربوط به ماه استخدام در شماره کارمندی</param>
        /// <returns>bنتیجه برسیool</returns>
        private static bool isMonthPartInValidRange(int monthPart)
        {
            return monthPart >= 0 && monthPart <= 12;
        }

        /// <summary>
        /// صحت قالب رشته ورودی را برسی می کند
        /// </summary>
        /// <param name="input">رشته ورودی</param>
        /// <returns>نتیجه برسی</returns>
        private static bool isValidFormat(string input)
        {
            Match match = Regex.Match(input, @"^(?<Y>\d{2})(?<M>\d{2})(?<S>\d{6})$");
            if (!match.Success)
                return false;

            try
            {
                int monthPart = int.Parse(match.Groups["M"].Value);
                int yearPart = int.Parse(match.Groups["Y"].Value);
                int serialPart = int.Parse(match.Groups["S"].Value);
                if (!isYearPartInValidRange(yearPart))
                    return false;
                if (!isMonthPartInValidRange(monthPart))
                    return false;
                if (!isSerialPartInValidRange(serialPart))
                    return false;
            }

            catch (FormatException)
            {
                return false;
            }


            return true;
        }

        /// <summary>
        /// از رشته ورودی یک نمونه از EmployeeID می سازد.
        /// </summary>
        /// <param name="input">رشته ورودی</param>
        /// <exception cref="InvalidEmployeeIDFormatException" />
        /// <returns>EmployeeID</returns>
        /// <see cref="isValidFormat(EmployeeID,DateTime)"/>
        public static EmployeeID Parse(string input)
        {

            if (!isValidFormat(input))
                throw new InvalidEmployeeIDFormatException();

            int yearPart = int.Parse(input.Substring(0, 2));
            int monthPart = int.Parse(input.Substring(2, 2));
            int serialPart = int.Parse(input.Substring(4, 6));

            return new EmployeeID(yearPart, monthPart, serialPart);
        }

        /// <summary>
        /// از رشته ورودی یک نمونه از EmployeeID می سازد. و موفقیت یا عدم موفقیت را بر می گرداند
        /// </summary>
        /// <param name="input">رشته ورودی</param>
        /// <param name="result">نتیجه</param>
        /// <returns>نتیجه Parse</returns>
        public static bool TryParse(string input, out EmployeeID result)
        {

            result = null;
            try
            {
                result = Parse(input);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// برسی صحت شماره کارمندی
        /// </summary>
        /// <param name="employeeIDinput">شماره کارمندی</param>
        /// <param name="dateOfEmployement">تاریخ استخدام کارمند</param>
        /// <returns>نتیجه برسی</returns>
        public static bool IsValid(EmployeeID employeeIDinput, DateTime dateOfEmployement)
        {
            ///[Validating EmployeeID]
            GregorianCalendar greogianCalendar = new GregorianCalendar();

            int l_yearPart = int.Parse(greogianCalendar.GetYear(dateOfEmployement).ToString().Remove(0, 2));
            int l_monthPart = greogianCalendar.GetMonth(dateOfEmployement);

            return employeeIDinput.m_yearPart == l_yearPart &&
                   dateOfEmployement.Month == l_monthPart;
            ///[Validating EmployeeID]
        }

        /// <summary>
        /// تبدیل شماره کارمندی به یک رشته، جهت ذخیره سازی و نمایش
        /// </summary>
        /// <returns>string</returns>
        private string serialize()
        {
            return m_yearPart.ToString().PadLeft(2, '0') + m_monthPart.ToString().PadLeft(2, '0') + m_serialPart.ToString().PadLeft(6, '0');
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

            if ((EmployeeID)obj == null)
                return false;

            EmployeeID employeeId1 = this;
            EmployeeID employeeId2 = (EmployeeID)obj;

            return (employeeId1.m_yearPart == employeeId2.m_yearPart)
                && (employeeId1.m_monthPart == employeeId2.m_monthPart)
                && (employeeId1.m_serialPart == employeeId2.m_serialPart);
        }

        /// <summary>
        /// مقدار HashCode نمونه جاری
        /// </summary>
        /// <returns>مقدار HashCode</returns>
        public override int GetHashCode()
        {

            Func<object, int> getHashCode = (obj) => obj != null ? obj.GetHashCode() : 0;
            int hashcode = 0;

            const int num = 23;

            hashcode *= num + getHashCode(this.m_yearPart);
            hashcode *= num + getHashCode(this.m_monthPart);
            hashcode *= num + getHashCode(this.m_serialPart);


            return hashcode;
        }

        /// <summary>
        /// عملگر ==
        /// </summary>
        /// <param name="a">نمونه اول</param>
        /// <param name="b">نمونه دوم</param>
        /// <returns>نتیجه برسی</returns>
        /// <seealso cref="!="/>
        public static bool operator ==(EmployeeID a, EmployeeID b)
        {
            if (Object.ReferenceEquals(a, b))
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
        public static bool operator !=(EmployeeID a, EmployeeID b)
        {
            return !(a == b);
        }

        /// <summary>
        /// نمایش رشته متناظر با EmployeeID
        /// </summary>
        /// <returns>رشته متناظر</returns>
        /// <seealso cref="serialize"/>
        public override string ToString()
        {
            return serialize();
        }

        /// <summary>
        /// ایجاد یک کپی کامل از نمونه جاری
        /// </summary>
        /// <returns>نمونه کپی شده</returns>
        public object Clone()
        {
            return new EmployeeID(m_yearPart, m_monthPart, m_serialPart);
        }
    }
}
