using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAS.Entities;
using NHibernate.Mapping.ByCode;
using AAS.Persistence.UserTypes;
namespace AAS.Persistence.Mappings
{
    /// <summary>
    /// کلاس ORM برای <c>AttendanceTime</c>
    /// </summary>
    public class AttendanceTimeMap : ClassMapping<AttendanceTime>
    {
        /// <summary>
        /// متد سازنده
        /// </summary>
        public AttendanceTimeMap()
        {
            Table("AttendanceTimes");
            Id(x => x.ID, m => m.Generator(Generators.Guid));
            Property(x => x.EntryTime);
            Property(x => x.ExitTime);
            ManyToOne(x => x.Employee, map => 
            { 
                map.Column("EmployeeId");
                map.Lazy(LazyRelation.NoLazy);
            });
            
        }
    }
}
