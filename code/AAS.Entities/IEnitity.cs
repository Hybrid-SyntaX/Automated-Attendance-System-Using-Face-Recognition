using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace AAS.Entities
{
    /// <summary>
    /// نمایانگر یک موجودیت در پایگاه داده. کلاس هایی که این interface را پیاده سازی می کنند، می توانند در پایگاه داده ذخیره شوند.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// شناسه داخلی قابل استفاده در پایگاه داده
        /// </summary>
        Guid ID { get; set; }
        
    }
}
