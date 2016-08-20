using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using AAS.Entities;
using AAS.Persistence.UserTypes;
namespace AAS.Persistence.Mappings
{
    /// <summary>
    /// کلاس ORM برای <c>Employee</c>
    /// </summary>
    public class EmployeeMap : ClassMapping<Employee>
    {
        /// <summary>
        /// متد سازنده
        /// </summary>
        public EmployeeMap()
        {

            Table("Employees");
            Id(x => x.ID, map => map.Generator(Generators.Guid));
            Property(x => x.FirstName);
            Property(x => x.LastName);
            Property(x => x.Gender);
            Property(x => x.DateOfBirth, m => { m.Type(NHibernateUtil.Date); m.Column(c => c.SqlType("DATE")); });
            Property(x => x.DateOfEmployement, m => { m.Type(NHibernateUtil.Date); m.Column(c => c.SqlType("DATE")); });
            Property(x => x.IdentityNumber);
            Property(x => x.NationalID, m => m.Type<ParsableType<IRNationalID>>());
            Property(x => x.EmployeeID, m => m.Type<ParsableType<EmployeeID>>());
            Property(x => x.WorkSchedule, m => m.Type<ParsableType<WorkSchedule>>());


            Bag(x => x.ContactInformations, map =>
            {
                map.Key(km => km.Column("EmployeeID"));
                map.Cascade(Cascade.All | Cascade.DeleteOrphans);
                map.Lazy(CollectionLazy.NoLazy);
                
            }, action => action.OneToMany());

            Bag(x => x.AttendanceTimes, map =>
            {
                map.Key(km => km.Column("EmployeeID"));
                map.Cascade(Cascade.All | Cascade.DeleteOrphans);
                map.Lazy(CollectionLazy.NoLazy);

            }, action => action.OneToMany());

        }
    }
}
