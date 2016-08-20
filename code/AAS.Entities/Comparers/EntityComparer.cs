using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAS.Entities.Comparers
{
    /// <summary>
    /// یک Comparer برای مقایسه کلاس هایی که IEnitty را پیاده سازی کردن اند و در پایگاه داده وجود دارند.
    /// </summary>
    /// <typeparam name="TBase">نوعی که IEnitty را پیاده سازی کرده است</typeparam>
    public class EntityComparer<TBase> : EqualityComparer<TBase> where TBase : IEntity
    {
        /// <summary>
        /// برسی همسان بودن
        /// </summary>
        /// <param name="x">نمونه اول</param>
        /// <param name="y">نمونه دوم</param>
        /// <returns>نتیجه برسی</returns>
        public override bool Equals(TBase x, TBase y)
        {
            return x.ID.Equals(y.ID);
        }

        /// <summary>
        /// گرفتن مقدار Hash
        /// </summary>
        /// <param name="obj">نمونه مورد نظر</param>
        /// <returns>HashCode نمونه ورودی</returns>
        public override int GetHashCode(TBase obj)
        {
            return obj.ID.GetHashCode();
        }
    }
    
}
