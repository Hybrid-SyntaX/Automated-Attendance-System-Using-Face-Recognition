using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAS.Util;
using AAS.Entities.Exceptions;
using System.Text.RegularExpressions;
using AAS.Entities.Resources;
using AAS.Entities.Interfaces;
namespace AAS.Entities
{
    /// <summary>
    /// اطلاعات تماس
    /// </summary>
    public class ContactInformation : IContactInformation, IEntity, ICloneable
    {
        private Guid m_id;

        /// <summary>
        /// شناسه داخلی قابل استفاده در پایگاه داده
        /// </summary>
        public virtual Guid ID
        {
            set
            {
                m_id = value;
            }
            get
            {
                return m_id;
            }
        }

        private Employee m_employee;
        /// <summary>
        /// کارمند مرتبط به این اطلاعات تماس
        /// </summary>
        /// <exception cref="RequiredFieldException">مقدار ورودی خالی باشد</exception>
        public virtual Employee Employee
        {
            set
            {
                if (value == null)
                    throw new RequiredFieldException("Employee");

                m_employee = value;
            }
            get
            {

                return m_employee;
            }
        }

        private string m_label;
        /// <summary>
        /// برچسب
        /// </summary>
        /// <exception cref="RequiredFieldException">مقدار ورودی خالی باشد</exception>
        /// <exception cref="InvalidFormatException">ورودی حاوی حروف غیر مجاز باشد</exception>
        public virtual string Label
        {
            set
            {
                if (Validator.IsNullOrEmptyOrWhitespace(value))
                    throw new RequiredFieldException("Label");
                if (!Validator.IsAlphabetWithSpaceInBetween(value))
                    throw new InvalidFormatException();

                m_label = value;
            }
            get
            {
                return m_label;
            }
        }

        private string m_phoneNumber;
        /// <summary>
        /// شماره تلفن
        /// </summary>
        /// <value>m_phoneNumber</value>
        /// <exception cref="RequiredFieldException">مقدار ورودی خالی باشد</exception>
        /// <exception cref="InvalidFormatException">ورودی حاوی حروف غیر مجاز باشد</exception>
        public virtual string PhoneNumber
        {
            set
            {
                if (Validator.IsNullOrEmptyOrWhitespace(value))
                    throw new RequiredFieldException("PhoneNumber");
                if (!Regex.IsMatch(value, RegExPatterns.PhoneNumber))
                    throw new InvalidFormatException();
                m_phoneNumber = value;
            }
            get
            {
                return m_phoneNumber;
            }
        }

        private string m_cellphoneNumber;
        /// <summary>
        /// شماره تلفن همراه
        /// </summary>
        /// <value>m_cellphoneNumber</value>
        /// <exception cref="RequiredFieldException">مقدار ورودی خالی باشد</exception>
        /// <exception cref="InvalidFormatException">ورودی حاوی حروف غیر مجاز باشد</exception>
        public virtual string CellphoneNumber
        {
            set
            {

                if (Validator.IsNullOrEmptyOrWhitespace(value))
                    throw new RequiredFieldException("CellPhoneNumber");
                if (!Regex.IsMatch(value, RegExPatterns.CellphoneNumber))
                    throw new InvalidFormatException();

                m_cellphoneNumber = value;
            }
            get
            {
                return m_cellphoneNumber;
            }
        }

        private string m_email;
        /// <summary>
        /// پست الکترونیک
        /// </summary>
        /// <value>m_email</value>
        /// <exception cref="RequiredFieldException">مقدار ورودی خالی باشد</exception>
        /// <exception cref="InvalidFormatException">ورودی حاوی حروف غیر مجاز باشد</exception>
        public virtual string Email
        {
            set
            {
                if (Validator.IsNullOrEmptyOrWhitespace(value))
                    throw new RequiredFieldException("Email");
                if (!Regex.IsMatch(value, RegExPatterns.EmailFormat))
                    throw new InvalidFormatException();

                m_email = value;
            }
            get
            {
                return m_email;
            }
        }

        private string m_address;

        /// <summary>
        /// آدرس
        /// </summary>
        /// <value>m_address</value>
        /// <exception cref="RequiredFieldException">مقدار ورودی خالی باشد</exception>
        /// <exception cref="InputTooShortException">اندازه ورودی کوچک باشد</exception>
        public virtual string Address
        {
            set
            {
                if (Validator.IsNullOrEmptyOrWhitespace(value))
                    throw new RequiredFieldException("Address");
                if (!Validator.IsNullOrEmptyOrWhitespace(value) && value.Length < 5)
                    throw new InputTooShortException();

                m_address = value;
            }
            get
            {
                return m_address;
            }
        }

        private string m_postalCode;

        /// <summary>
        /// کد پستی
        /// </summary>
        /// <exception cref="RequiredFieldException">مقدار ورودی خالی باشد</exception>
        /// <exception cref="InvalidFormatException">ورودی حاوی حروف غیر مجاز باشد @cite fa_wiki_postalcode</exception>
        /// <value>m_postalCode</value>
        public virtual string PostalCode
        {
            set
            {
                if (Validator.IsNullOrEmptyOrWhitespace(value))
                    throw new RequiredFieldException("PostalCode");
                if (!Regex.IsMatch(value, RegExPatterns.PosalCode))
                    throw new InvalidFormatException("Length of PostalCode is 10 and should not contain 2 or 0");
                m_postalCode = value;
            }
            get
            {
                return m_postalCode;
            }
        }

        /// <summary>
        /// نمایش رشته ای ContactInfomration
        /// </summary>
        /// <returns>رشته متناظر</returns>
        public override string ToString()
        {
            return Label;
        }

        /// <summary>
        /// عملگر ==
        /// </summary>
        /// <param name="A">نمونه اول</param>
        /// <param name="B">نمونه دوم</param>
        /// <returns>نتیجه برسی</returns>
        public static bool operator ==(ContactInformation A, ContactInformation B)
        {
            ///[Contact Information EqualityOperator]
            if (object.ReferenceEquals(A, B))
                return true;

            if ((object)A == null || (object)B == null)
                return false;

            return A.Equals(B);
            ///[Contact Information EqualityOperator]
        }

        /// <summary>
        /// عملگر !=
        /// </summary>
        /// <param name="A">نمونه اول</param>
        /// <param name="B">نمونه دوم</param>
        /// <returns>نتیجه برسی</returns>
        public static bool operator !=(ContactInformation A, ContactInformation B)
        {
            return !(A == B);
        }

        /// <summary>
        /// مقدار HashCode نمونه جاری
        /// </summary>
        /// <returns>مقدار HashCode</returns>
        public override int GetHashCode()
        {
            ///[ContactInformation GetHashCode]
            Func<object, int> getHashCode = (obj) => obj != null ? obj.GetHashCode() : 0;
            int hashcode = 0;

            const int num = 23;


            hashcode *= num + getHashCode(ID);
            hashcode *= num + getHashCode(Label);
            hashcode *= num + getHashCode(PhoneNumber);
            hashcode *= num + getHashCode(CellphoneNumber);
            hashcode *= num + getHashCode(Email);
            hashcode *= num + getHashCode(Address);
            hashcode *= num + getHashCode(PostalCode);


            return hashcode;
            ///[ContactInformation GetHashCode]
        }

        /// <summary>
        /// برسی یکسان بودن نمونه جاری با نمونه ورودی
        /// </summary>
        /// <param name="obj">نمونه وردی</param>
        /// <returns>نتیجه برسی</returns>
        public override bool Equals(object obj)
        {
            ///[ContactInformation Equals]
            bool areEqual = true;
            if (obj == null)
                return false;

            if (!(obj is ContactInformation))
                return false;
            ContactInformation A = this;
            ContactInformation B = (ContactInformation)obj;

            areEqual &= A.ID == B.ID;
            areEqual &= A.Label == B.Label;
            areEqual &= A.PhoneNumber == B.PhoneNumber;
            areEqual &= A.CellphoneNumber == B.CellphoneNumber;
            areEqual &= A.Email == B.Email;
            areEqual &= A.Address == B.Address;
            areEqual &= A.PostalCode == B.PostalCode;

            return areEqual;
            ///[ContactInformation Equals]  
        }

        /// <summary>
        /// کپی کردن از نمونه جاری
        /// </summary>
        /// <returns>object</returns>
        public virtual object Clone()
        {
            ///[ContactInformation Clone]
            return new ContactInformation()
            {
                ID = this.ID,
                Label = this.Label,
                PhoneNumber = this.PhoneNumber,
                CellphoneNumber = this.CellphoneNumber,
                Email = this.Email,
                Address = this.Address,
                PostalCode = this.PostalCode
            };
            ///[ContactInformation Clone]
        }
    }
}
