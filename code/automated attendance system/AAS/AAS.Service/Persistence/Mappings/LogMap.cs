using AAS.Entities;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAS.Persistence.Mappings
{
    /// <summary>
    /// کلاس ORM برای <c>Log</c>
    /// </summary>
    public class LogMap:ClassMapping<Log>
    {
        /// <summary>
        /// متد سازنده
        /// </summary>
        public LogMap()
        {
            Table("Logs");
            Id(x => x.ID, m => m.Generator(Generators.Guid));
            Property(x => x.AttendanceMethod);
            Property(x => x.AttendanceMethodResult);
            Property(x => x.EventTime);
            Property(x => x.EnteredEmployeeID);

            
            ManyToOne(x => x.Employee, map =>
            {

                map.Column("EmployeeId");
                map.NotNullable(false);
                map.Class(typeof(Employee));
                map.Lazy(LazyRelation.NoLazy);
            });
        }
    }
}
