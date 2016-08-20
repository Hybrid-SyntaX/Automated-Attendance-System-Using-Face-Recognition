using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAS.Entities.Exceptions;
using AAS.Util;
using System.Collections;
using System.Collections.Specialized;
using System.ServiceModel;
using AAS.Entities.Resources;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
namespace AAS.Entities
{
    /// <summary>
    /// ساعات و روز های کاری
    /// </summary>
    [DataContract]
    public class WorkSchedule : ICloneable,IEnumerable
    {

        /// <summary>
        /// وضعیت کاری
        /// </summary>
        public enum State
        {
            /// <summary>
            /// ساعات غیر کاری
            /// </summary>
            Off = 0,

            /// <summary>
            /// ساعات کاری
            /// </summary>
            Work = 1

        };

        [DataMember]
        private Dictionary<DayOfWeek, BitArray> m_daysInWeek;

        /// <summary>
        /// کمترین مقداری که WorkSchedule می تواند  داشته باشد
        /// </summary> 
        public static WorkSchedule MinValue
        {
            get
            {
                return new WorkSchedule();
            }
        }
       
        /// <summary>
        /// بیشترین مقداری که WorkSchedule می تواند  داشته باشد
        /// </summary>
        public static WorkSchedule MaxValue
        {
            get
            {
                WorkSchedule maxworkschedule = new WorkSchedule();

                foreach (var daysInWeek in maxworkschedule.m_daysInWeek.Values)
                {
                    daysInWeek.SetAll(true);
                }
                return maxworkschedule;
            }
        }

        /// <summary>
        /// متد سازنده WorkSchedule
        /// </summary>
        public WorkSchedule()
        {

            m_daysInWeek = new Dictionary<DayOfWeek, BitArray>();
            m_daysInWeek.Add(DayOfWeek.Saturday, new BitArray(24, false));
            m_daysInWeek.Add(DayOfWeek.Sunday, new BitArray(24, false));
            m_daysInWeek.Add(DayOfWeek.Monday, new BitArray(24, false));
            m_daysInWeek.Add(DayOfWeek.Tuesday, new BitArray(24, false));
            m_daysInWeek.Add(DayOfWeek.Wednesday, new BitArray(24, false));
            m_daysInWeek.Add(DayOfWeek.Thursday, new BitArray(24, false));
            m_daysInWeek.Add(DayOfWeek.Friday, new BitArray(24, false));
        }

        /// <summary>
        /// تبدیل رشته ورودی به یک WorkSchedule
        /// </summary>
        /// <param name="s">رشته ورودی</param>
        /// <exception cref="InvalidFormatException" >فرمت نامعتبر است</exception>
        /// <exception cref="ArgumentNullException" >ورودی خالی داده شده</exception>
        /// <returns>workschedule</returns>  
        public static WorkSchedule Parse(string s)
        {
            
            ///[Preparing for parse]
            if (!Validator.IsMatch(s, RegExPatterns.WorkSchedule, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                throw new InvalidFormatException();

            WorkSchedule workschedule = new WorkSchedule();
            string[] daysInWeek = s.Split('-');
            ///[Preparing for parse]
            int i = 0;
            ///[Converting each day value from hexadecimal to binary]
            Dictionary<DayOfWeek, BitArray> temp = new Dictionary<DayOfWeek, BitArray>(workschedule.m_daysInWeek);
            foreach (var weekDay in workschedule.m_daysInWeek.Keys)
            {
                string hoursInDay = Convert.ToString(Convert.ToInt32(daysInWeek[i], 16), 2).PadLeft(24, '0');
                temp[weekDay] = convertStringToBitarray(hoursInDay);
                i++;
            }
            workschedule.m_daysInWeek = temp;
            return workschedule;
            ///[Converting each day value from hexadecimal to binary]
            
        }

        /// <summary>
        /// تبدیل WorkSchedule به رشته برای ذخیره سازی
        /// </summary>
        /// <returns>string</returns>
        private string serialize()
        {

            ///[Serializing WorkSchedule]

            string result = "";

            foreach (var hoursInDay in m_daysInWeek.Values)
            {
                result += hoursInDay.ToHexadecimalString() + "-";
            }
            result = result.Remove(result.LastIndexOf("-"));

            return result;

            ///[Serializing WorkSchedule]
        }

        /// <summary>
        /// تعیین ساعت کاری در روز
        /// </summary>
        /// <param name="dayOfWeek">روز هفته</param>
        /// <param name="begin">ساعت شروع</param>
        /// <param name="end">ساعت پایان</param>
        /// <param name="state">وضعیت کاری</param>
        /// <exception cref="InvalidInputException" >ورودی نامعتبر است</exception>
        public void DefineRange(DayOfWeek dayOfWeek, int begin, int end, State state)
        {
            ///[Defining range]
            checkHourForValidRangeOrThrowArguementOutOfRangeException(begin);
            checkHourForValidRangeOrThrowArguementOutOfRangeException(end);

            end = ((end == 0) ? 24 : end) - 1;

            if (end < begin)
                throw new InvalidInputException(string.Format("Begin({0}) is bigger than end({1})", begin, end));


            for (int i = begin; i <= end; i++)
            {
                m_daysInWeek[dayOfWeek].Set(i, state.ToBoolean());
            }

            ///[Defining range]
        }

        /// <summary>
        /// تعیین یک ساعت به عنوان ساعت کاری
        /// </summary>
        /// <param name="dayOfWeek">روز هفته</param>
        /// <param name="hour">ساعت</param>
        /// <param name="state">وضعیت کاری</param>
        /// <exception cref="ArgumentOutOfRangeException" >ورودی خارج از محدوده معتبر است</exception>
        public void Define(DayOfWeek dayOfWeek, int hour, State state)
        {
            ///[Defining a hour]
            checkHourForValidRangeOrThrowArguementOutOfRangeException(hour);
            m_daysInWeek[dayOfWeek].Set(hour, state.ToBoolean());
            ///[Defining a hour]
        }

        /// <summary>
        /// اندیس ساز برای دسترسی به مقادیر موجود در workschedule
        /// </summary>
        /// <param name="index">اندیس</param>
        /// <returns>BitArray</returns>
        public BitArray this[DayOfWeek index]
        {
            get
            {
                return m_daysInWeek[index];
            }
        }

        /// <summary>
        /// تبدیل یک رشته حاوی اعداد باینری به BitArray
        /// </summary>
        /// <param name="s">رشته ورودی</param>
        /// <returns>BitArray</returns>
        private static BitArray convertStringToBitarray(string s)
        {
            BitArray result = new BitArray(24, false);
            for (int i = 0; i < s.Length; i++)
            {
                result.Set(i, s[i] == '1' ? true : false);
            }

            return result;
        }

        /// <summary>
        /// برسی ساعت
        /// </summary>
        /// <param name="hour"></param>
        /// <exception cref="ArgumentOutOfRangeException" >ورودی خارج از محدوده معتبر است</exception>
        private static void checkHourForValidRangeOrThrowArguementOutOfRangeException(int hour)
        {
            if (hour > 23 || hour < 0)
                throw new ArgumentOutOfRangeException("hour", hour, string.Format("Hour must be between 0 and 23 but it was {0}", hour));
        }
    
        /// <summary>
        /// ساعت شروع کار
        /// </summary>
        /// <param name="dayOfWeek">روز هفته</param>
        /// <returns>ساعت شروع کار</returns>
        public int GetStartHour(DayOfWeek dayOfWeek)
        {
            ///[GetStartHour]
            int i = 0;
            while (i < m_daysInWeek[dayOfWeek].Count && this[dayOfWeek][i] != true)
                i++;
            return i;
            ///[GetStartHour]
        }
       
        /// <summary>
        /// ساعت پایان کار
        /// </summary>
        /// <param name="dayOfWeek">روز هفته</param>
        /// <returns>روز هفته</returns>
        public int GetEndHour(DayOfWeek dayOfWeek)
        {
            ///[GetEndHour]
            int i = m_daysInWeek[dayOfWeek].Count-1;
            while (i > 0 && m_daysInWeek[dayOfWeek][i] != true)
                i--;
            return i+1;
            ///[GetEndHour]
        }

        /// <summary>
        /// دریافت IEnumerator مروبط به این نوع 
        /// </summary>
        /// <returns>IEnumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            ///[Getting IEnumerator]
            List<DayOfWeek> listOfDays = new List<DayOfWeek>() { DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };

            var query = from day in listOfDays select new { Day = day, Hours = (this[day].ToList()) };
            return query.GetEnumerator();
            ///[Getting IEnumerator]

        }

        /// <summary>
        /// مشخیص کننده وضعیت به روز WorkSchedule
        /// </summary>
        public bool Updated { set; get; }
       
        /// <summary>
        /// پاک کردن تمام روز های موجود
        /// </summary>
        public void Reset()
        {
            foreach (var daysInWeek in m_daysInWeek.Values)
            {
                daysInWeek.SetAll(false);
            }
        }

        /// <summary>
        /// ایجاد یک کپی کامل از نمونه جاری
        /// </summary>
        /// <returns>نمونه کپی شده</returns>
        public object Clone()
        {
            return WorkSchedule.Parse(this.serialize());
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

            for (int i = 0; i < 24; i++)
            {
                hashcode *= num + getHashCode(m_daysInWeek[DayOfWeek.Saturday][i]);
                hashcode *= num + getHashCode(m_daysInWeek[DayOfWeek.Sunday][i]);
                hashcode *= num + getHashCode(m_daysInWeek[DayOfWeek.Monday][i]);
                hashcode *= num + getHashCode(m_daysInWeek[DayOfWeek.Tuesday][i]);
                hashcode *= num + getHashCode(m_daysInWeek[DayOfWeek.Wednesday][i]);
                hashcode *= num + getHashCode(m_daysInWeek[DayOfWeek.Thursday][i]);
                hashcode *= num + getHashCode(m_daysInWeek[DayOfWeek.Friday][i]);
            }
            return hashcode;
        }

        /// <summary>
        /// برسی یکسان بودن نمونه جاری با نمونه ورودی
        /// </summary>
        /// <param name="obj">نمونه وردی</param>
        /// <returns>نتیجه برسی</returns>
        public override bool Equals(object obj)
        {
            bool areEqual = true;
            if (obj == null)
                return false;

            if (!(obj is WorkSchedule))
                return false;

            WorkSchedule a = this;
            WorkSchedule b = obj as WorkSchedule;


            for (int i = 0; i < 24; i++)
            {
                areEqual &= a.m_daysInWeek[DayOfWeek.Saturday][i] == b.m_daysInWeek[DayOfWeek.Saturday][i];
                areEqual &= a.m_daysInWeek[DayOfWeek.Sunday][i] == b.m_daysInWeek[DayOfWeek.Sunday][i];
                areEqual &= a.m_daysInWeek[DayOfWeek.Monday][i] == b.m_daysInWeek[DayOfWeek.Monday][i];
                areEqual &= a.m_daysInWeek[DayOfWeek.Tuesday][i] == b.m_daysInWeek[DayOfWeek.Tuesday][i];
                areEqual &= a.m_daysInWeek[DayOfWeek.Wednesday][i] == b.m_daysInWeek[DayOfWeek.Wednesday][i];
                areEqual &= a.m_daysInWeek[DayOfWeek.Thursday][i] == b.m_daysInWeek[DayOfWeek.Thursday][i];
                areEqual &= a.m_daysInWeek[DayOfWeek.Friday][i] == b.m_daysInWeek[DayOfWeek.Friday][i];
            }

            return areEqual;
        }

        /// <summary>
        /// عملگر ==
        /// </summary>
        /// <param name="a">نمونه اول</param>
        /// <param name="b">نمونه دوم</param>
        /// <returns>نتیجه برسی</returns>
        /// <seealso cref="!="/>
        public static bool operator ==(WorkSchedule a, WorkSchedule b)
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
        public static bool operator !=(WorkSchedule a, WorkSchedule b)
        {
            return !(a == b);
        }


        /// <summary>
        /// نمایش رشته متناظر با WorkSchedule
        /// </summary>
        /// <returns>رشته متناظر</returns>
        /// <seealso cref="serialize"/>
        public override string ToString()
        {
            return serialize();
        }

    }
}
