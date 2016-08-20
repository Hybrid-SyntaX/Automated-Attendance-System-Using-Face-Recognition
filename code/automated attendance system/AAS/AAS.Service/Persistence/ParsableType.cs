using NHibernate;
using NHibernate.UserTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAS.Persistence.UserTypes
{
    /// <summary>
    /// یک نوع ایجاد شده برای NHibernate جهت ذخیره سازی مقادیر
    /// <c>WorkSchedule</c> , <c>IRNationalID</c> , <c>EmployeeID</c>
    /// </summary>
    /// <typeparam name="T">نوع مورد نظر</typeparam>
    public class ParsableType<T> : IUserType where T : ICloneable, new()
    {
        /// <summary>
        /// بازسازی از روی کش
        /// </summary>
        /// <param name="cached">نمونه ای که قرار از کش شود</param>
        /// <param name="owner">مالک نمونه کش شده</param>
        /// <returns>نمونه بازسازی شده از کش</returns>
        public virtual object Assemble(object cached, object owner)
        {
            return cached;
        }
        
        /// <summary>
        /// آماده سازی نمونه برای کش شدن
        /// </summary>
        /// <param name="value">نمونه ای که قرار است کش شود</param>
        /// <returns>یک نمونه قابل کش</returns>
        public virtual object Disassemble(object value)
        {
            return value;
        }

        /// <summary>
        /// ایجاد یک کپی
        /// </summary>
        /// <param name="value">مقداری که باید کپی شود</param>
        /// <returns>کپی مقدار ورودی</returns>
        public virtual object DeepCopy(T value)
        {
            return value.Clone();
        }

        /// <summary>
        /// ایجاد یک کپی
        /// </summary>
        /// <param name="value">مقداری که باید کپی شود</param>
        /// <returns>کپی مقدار ورودی</returns>
        public virtual object DeepCopy(object value)
        {
            return DeepCopy((T)value);
        }


        /// <summary>
        /// برسی همسان بودن
        /// </summary>
        /// <param name="x">نمونه اول</param>
        /// <param name="y">نمونه دوم</param>
        /// <returns>نتیجه مقایسه</returns>
        public virtual new bool Equals(object x, object y)
        {
            if (x == null)
            {
                return false;
            }
            else
            {
                return x.Equals(y);
            }
        }

        /// <summary>
        /// مقدار hash
        /// </summary>
        /// <param name="x">نمونه مورد نظر</param>
        /// <returns>hashcode نمونه ورودی</returns>
        public virtual int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        /// <summary>
        /// قابل تغییر است (Mutable)
        /// </summary>
        public virtual bool IsMutable
        {
            get { return false; }
        }

        /// <summary>
        /// یک نمنه کلاس Map شده را بر می گرداند
        /// </summary>
        /// <param name="rs">یک IDataReader</param>
        /// <param name="names">نام ستون ها</param>
        /// <param name="owner">مالک این موجودیت</param>
        public virtual object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            ///[NullSafeGet body]
            string stringValue = (string)NHibernateUtil.String.NullSafeGet(rs, names[0]);

            T result = new T();

            System.Reflection.MethodInfo parseMethod = typeof(T).GetMethod("Parse");

            if (parseMethod != null)
                result = (T)parseMethod.Invoke(null, new object[] { stringValue });


            return result;
            ///[NullSafeGet body]
        }

        /// <summary>
        /// نوشتن یک کلاس Map شده
        /// </summary>
        /// <param name="cmd">یک IDbCommand</param>
        /// <param name="value">نمونه ای باید نوشته شود</param>
        /// <param name="index">اندیس برای پارامتر Command</param>
        public virtual void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            ///[NullSafeSet Body]
            if (value == null)
            {
                NHibernateUtil.String.NullSafeSet(cmd, null, index);
                return;
            }
            value = value.ToString();
            NHibernateUtil.String.NullSafeSet(cmd, value, index);
            ///[NullSafeSet Body]
        }

        /// <summary>
        /// جایگزینی مقدار جاری با جدید
        /// </summary>
        /// <param name="original">مقدار جدید</param>
        /// <param name="target">مقدار جاری</param>
        /// <param name="owner">موجودیت مدیریت شده</param>
        /// <returns>مقداری که باید ادغام شود</returns>
        public virtual object Replace(object original, object target, object owner)
        {
            return original;
        }

        /// <summary>
        /// نوع خروجی
        /// </summary>
        public virtual Type ReturnedType
        {
            get { return typeof(T); }
        }

        /// <summary>
        /// نوع SQL متناظر
        /// </summary>
        public virtual NHibernate.SqlTypes.SqlType[] SqlTypes
        {
            get
            {
                return new NHibernate.SqlTypes.SqlType[] { new NHibernate.SqlTypes.SqlType(DbType.String) };
            }
        }
    }
}   
