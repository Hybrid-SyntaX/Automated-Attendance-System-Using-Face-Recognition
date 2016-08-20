using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using AAS.Entities.Exceptions;
using AAS.Util;
using System.Drawing;
using System.Runtime.Serialization;
using AAS.Entities.Interfaces;
using System.Globalization;
namespace AAS.Entities
{
    /// <summary>
    /// جنسیت
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// آقا
        /// </summary>
        Male = 0,
        /// <summary>
        /// خانم
        /// </summary>
        Female = 1
    };

    /// <summary>
    /// کلاس کارمند
    /// </summary>
    [DataContract, KnownType(typeof(Employee))]
    public class Employee : IEmployee, IEntity, ICloneable
    {
        /// <summary>
        /// متد سازنده کارمند
        /// </summary>
        public Employee()
        {
            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        }
        
        private Guid m_id;
        /// <summary>
        /// شماره داخلی قابل استفاده در پایگاه داده
        /// </summary>
        [DataMember]
        public virtual Guid ID
        {
            set
            {
                //if (value == Guid.Empty)
                //    throw new RequiredFieldException("ID");

                m_id = value;
            }
            get
            {
                return m_id;
            }
        }

        private string m_firstName;
        /// <summary>
        /// نام
        /// </summary>
        /// <value>m_firstName</value>
        /// <exception cref="RequiredFieldException">مقدار ورودی خالی باشد</exception>
        /// <exception cref="InvalidFormatException">ورودی حاوی حروف غیر مجاز باشد</exception>
        [DataMember]
        public virtual string FirstName
        {
            set
            {

                if (Validator.IsNullOrEmptyOrWhitespace(value))
                {
                    throw new RequiredFieldException("FirstName");
                }
                if (!Regex.IsMatch(value, @"^\w+(\s+\w+)*$"))
                {
                    throw new InvalidFormatException();
                }
                m_firstName = value;
            }
            get
            {
                return m_firstName;
            }
        }

        private string m_lastName;
        /// <summary>
        /// نام خانوادگی
        /// </summary>
        /// <value>m_lastName</value>
        /// <exception cref="RequiredFieldException">مقدار ورودی خالی باشد</exception>
        /// <exception cref="InvalidFormatException">ورودی حاوی حروف غیر مجاز باشد</exception>
        [DataMember]
        public virtual string LastName
        {
            set
            {
                if (Validator.IsNullOrEmptyOrWhitespace(value))
                {
                    throw new RequiredFieldException("LastName");
                }
                if (!Regex.IsMatch(value, @"^\w+(\s+\w+)*$"))
                {
                    throw new InvalidFormatException();
                }
                m_lastName = value;
            }
            get
            {
                return m_lastName;
            }
        }

        /// <summary>
        /// نام کامل
        /// </summary>
        public virtual string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }
        private Gender m_gender;

        /// <summary>
        /// جنسیت
        /// </summary>
        /// <value>m_gender</value>
        [DataMember]
        public virtual Gender Gender
        {
            set
            {
                m_gender = value;
            }
            get
            {
                return m_gender;
            }
        }

        private DateTime m_dateOfBirth;
        /// <summary>
        /// تاریخ تولد
        /// </summary>
        /// <value>m_dateOfBirth</value>
        /// <exception cref="RequiredFieldException">مقدار ورودی خالی باشد</exception>
        /// <exception cref="InvalidDateException">تاریخ نامعتبر باشد</exception>
        /// <exception cref="IllegalAgeException">سن کمتر از 18 سال باشد</exception>
        /// <exception cref="InvalidFormatException">ورودی حاوی حروف غیر مجاز باشد</exception>
        [DataMember]
        public virtual DateTime DateOfBirth
        {
            set
            {


                if (value == DateTime.MinValue)
                    throw new RequiredFieldException("DateOfBirth");
                if (value >= DateTime.Today)
                    throw new InvalidDateException();
                if (DateTime.Today.Year - value.Year < 18)
                    throw new IllegalAgeException();
                if (DateTime.Today.Year - value.Year >= 200)
                    throw new InvalidFormatException();

                m_dateOfBirth = value;
            }
            get
            {
                return m_dateOfBirth;
            }
        }

        private DateTime m_dateOfEmployement;
        /// <summary>
        /// تاریخ استخدام
        /// </summary>
        /// <value>m_dateOfEmployement</value>
        /// <exception cref="RequiredFieldException">مقدار ورودی خالی باشد</exception>
        /// <exception cref="InvalidDateException">تاریخ نامعتبر باشد</exception>
        [DataMember]
        public virtual DateTime DateOfEmployement
        {
            set
            {
                if (value == DateTime.MinValue)
                    throw new RequiredFieldException("DateOfEmployement");
                if (value < DateOfBirth)
                    throw new InvalidDateException("DateOfEmployement Is Less Than DateOfBirth");
                if (value > DateTime.Now)
                    throw new InvalidDateException("DateOfEmployement Is Greater Than Today");

                m_dateOfEmployement = value;
            }
            get
            {
                return m_dateOfEmployement;
            }
        }

        private IRNationalID m_nationalID;
        /// <summary>
        /// کد ملی
        /// </summary>
        /// <value>m_nationalID</value>
        /// <exception cref="RequiredFieldException">مقدار ورودی خالی باشد</exception>
        /// <exception cref="InvalidIRNationalIDFormatException">فرمت کد ملی اشتباه باشد</exception>
        /// <exception cref="InvalidIRNationalIDException">کد ملی اشتباه باشد</exception>
        [DataMember]
        public virtual IRNationalID NationalID
        {
            set
            {
                if (value == null)
                    throw new RequiredFieldException("NationalID");

                if (value.Equals(IRNationalID.InvalidValue))
                    throw new InvalidIRNationalIDFormatException();

                if (value.Equals(IRNationalID.Empty))
                    throw new RequiredFieldException("NationalID");

                if (!IRNationalID.IsValid(value))
                    throw new InvalidIRNationalIDException();


                m_nationalID = value;


            }
            get
            {
                if (m_nationalID == null)
                    m_nationalID = new IRNationalID();
                return m_nationalID;
            }
        }

        private string m_identityNumber;
        /// <summary>
        /// شماره شناسنامه
        /// </summary>
        /// <value>m_identityNumber</value>
        /// <exception cref="RequiredFieldException">مقدار ورودی خالی باشد</exception>
        /// <exception cref="InvalidIdentityNumberException">شماره شناسنامه نا معتبر باشد</exception>
        [DataMember]
        public virtual string IdentityNumber
        {
            set
            {
                if (Validator.IsNullOrEmptyOrWhitespace(value))
                    throw new RequiredFieldException("IdentityNumber");

                if (!Regex.IsMatch(value, @"^\d+$"))
                    throw new InvalidIdentityNumberException();

                m_identityNumber = value;
            }
            get
            {

                return m_identityNumber;
            }
        }

        private EmployeeID m_employeeId;
        /// <summary>
        /// شماره کارمندی
        /// </summary>
        /// <value>m_employeeId</value>
        /// <exception cref="RequiredFieldException">مقدار ورودی خالی باشد</exception>
        /// <exception cref="InvalidEmployeeIDException">شماره کارمندی نامعتبر باشد</exception>
        [DataMember]
        public virtual EmployeeID EmployeeID
        {
            set
            {

                if (value.Equals(EmployeeID.Empty))
                    throw new RequiredFieldException("EmployeeID");
                if (!EmployeeID.IsValid(value, DateOfEmployement))
                    throw new InvalidEmployeeIDException();

                m_employeeId = value;
            }
            get
            {
                return m_employeeId;
            }
        }

        private Bitmap m_profilePicture;
        /// <summary>
        /// عکس پرسنلی
        /// </summary>
        /// <value>m_profilePicture</value>
        /// <exception cref="RequiredFieldException">مقدار ورودی خالی باشد</exception>
        [DataMember]
        public virtual Bitmap ProfilePicture
        {
            set
            {

                if (value == null)
                    throw new RequiredFieldException("ProfilePicture");
                m_profilePicture = value;
            }
            get
            {
                if (m_profilePicture == null)
                    m_profilePicture = new Bitmap(1, 1);
                return m_profilePicture;
            }
        }

        private IList<ContactInformation> m_contactInformations;
        /// <summary>
        /// اطلاعات تماس
        /// </summary>
        /// <value>m_contactInformations</value>
        [DataMember]
        public virtual IList<ContactInformation> ContactInformations
        {
            set
            {
                m_contactInformations = value;
            }
            get
            {
                if (m_contactInformations == null)
                    m_contactInformations = new List<ContactInformation>();

                return m_contactInformations;
            }
        }

        private IList<AttendanceTime> m_attendanceTimes;
        /// <summary>
        /// ساعات حضور
        /// </summary>
        /// <value>m_attendanceTimes</value>
        [DataMember]
        public virtual IList<AttendanceTime> AttendanceTimes
        {
            set
            {
                m_attendanceTimes = value;
            }
            get
            {
                if (m_attendanceTimes == null)
                    m_attendanceTimes = new List<AttendanceTime>();

                return m_attendanceTimes;
            }
        }

        private WorkSchedule m_workSchedule;
        /// <summary>
        /// ساعات کاری
        /// </summary>
        /// <value>m_workSchedule</value>
        [DataMember]
        public virtual WorkSchedule WorkSchedule
        {
            set
            {
                m_workSchedule = value;
            }
            get
            {
                if (m_workSchedule == null)
                    m_workSchedule = new WorkSchedule();

                return m_workSchedule;
            }
        }
        ///  <summary>
        ///  عملگر ==
        ///  </summary>
        ///  <param name="A">نمونه اول</param>
        ///  <param name="B">نمونه دوم</param>
        ///  <returns>نتیجه برسی</returns>
        public static bool operator ==(Employee A, Employee B)
        {
            if (object.ReferenceEquals(A, B))
                return true;

            if ((object)A == null || (object)B == null)
                return false;

            return A.Equals(B);
        }

        ///  <summary>
        ///  عملگر !=
        ///  </summary>
        ///  <param name="A">نمونه اول</param>
        ///  <param name="B">نمونه دوم</param>
        ///  <returns>نتیجه برسی</returns>
        /// <seealso cref="=="/>
        public static bool operator !=(Employee A, Employee B)
        {
            return !(A == B);
        }

        ///  <summary>
        ///  برسی یکسان بودن نمونه جاری با نمونه ورودی
        ///  </summary>
        ///  <param name="obj">نمونه وردی</param>
        ///  <returns>نتیجه برسی</returns>
        public override bool Equals(object obj)
        {
            bool areEqual = true;
            if (obj == null)
                return false;

            if (!(obj is Employee))
                return false;

            Employee A = (Employee)this;
            Employee B = (Employee)obj;

            areEqual &= A.ID.Equals(B.ID);
            areEqual &= A.FirstName == B.FirstName;
            areEqual &= A.LastName == B.LastName;
            areEqual &= A.Gender == B.Gender;
            areEqual &= A.DateOfBirth == B.DateOfBirth;
            areEqual &= IRNationalID.Equals(A.NationalID, B.NationalID);
            areEqual &= A.IdentityNumber == B.IdentityNumber;
            areEqual &= EmployeeID.Equals(A.EmployeeID, B.EmployeeID);
            areEqual &= WorkSchedule.Equals(A.WorkSchedule, B.WorkSchedule);

            // areEqual &= A.ProfilePicture ==B.ProfilePicture;


            areEqual &= A.ContactInformations.Count == B.ContactInformations.Count;
            for (int i = 0; areEqual && i < A.ContactInformations.Count; i++)
                areEqual &= ContactInformation.Equals(A.ContactInformations[i], B.ContactInformations[i]);

            return areEqual;
        }

        ///  <summary>
        ///  مقدار HashCode نمونه جاری
        ///  </summary>
        ///  <returns>مقدار HashCode</returns>
        public override int GetHashCode()
        {
            Func<object, int> getHashCode = (obj) => obj != null ? obj.GetHashCode() : 0;
            int hashcode = 0;

            const int num = 23;

            hashcode *= num + getHashCode(this.FirstName);
            hashcode *= num + getHashCode(this.LastName);
            hashcode *= num + getHashCode(this.Gender);
            hashcode *= num + getHashCode(this.DateOfBirth);
            hashcode *= num + getHashCode(this.NationalID);
            hashcode *= num + getHashCode(this.IdentityNumber);
            hashcode *= num + getHashCode(this.EmployeeID);
            //            hashcode *= num + getHashCode(this.ProfilePicture);
            hashcode *= num + getHashCode(this.WorkSchedule);
            hashcode *= num + getHashCode(this.ContactInformations.Count);
            for (int i = 0; i < this.ContactInformations.Count; i++)
                hashcode *= num + getHashCode(this.ContactInformations[i]);


            return hashcode;
        }

        /// <summary>
        /// نمایش رشته متناظر با نمونه جاری
        /// </summary>
        /// <returns>نمایش رشته متناظر</returns>
        public override string ToString()
        {
            return string.Format("{0} {1} ({2})", FirstName, LastName, EmployeeID);
        }

        /// <summary>
        /// ایجاد یک کپی کامل از نمونه جاری
        /// </summary>
        /// <returns>نمونه کپی شده</returns>
        public virtual object Clone()
        {
            return new Employee()
            {
                ID = this.ID,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Gender = this.Gender,
                IdentityNumber = this.IdentityNumber,
                DateOfBirth = this.DateOfBirth,
                WorkSchedule = (WorkSchedule)this.WorkSchedule.Clone(),
                DateOfEmployement = this.DateOfEmployement,
                NationalID = (IRNationalID)this.NationalID.Clone(),
                EmployeeID = (EmployeeID)this.EmployeeID.Clone(),
                ProfilePicture = (Bitmap)this.ProfilePicture.Clone(),
                ContactInformations = new List<ContactInformation>(this.ContactInformations.ToList<ContactInformation>())

            };
        }


    }
}
