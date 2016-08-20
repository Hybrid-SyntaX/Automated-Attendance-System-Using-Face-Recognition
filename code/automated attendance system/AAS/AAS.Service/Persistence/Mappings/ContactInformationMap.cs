using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;
using AAS.Entities;
using NHibernate.Mapping.ByCode;
using AAS.Entities.Interfaces;
namespace AAS.Persistence.Mappings
{
    /// <summary>
    /// کلاس ORM برای <c>ContactInformation</c>
    /// </summary>
    public class ContactInformationMap : ClassMapping<ContactInformation>
    {
        /// <summary>
        /// متد سازنده
        /// </summary>
        public ContactInformationMap()
        {

            Table("ContactInformations");
            Id(x => x.ID, m => m.Generator(Generators.Guid));
            Property(x => x.Label);
            Property(x => x.PhoneNumber);
            Property(x => x.CellphoneNumber);
            Property(x => x.Email);
            Property(x => x.Address);
            Property(x => x.PostalCode);
            ManyToOne(x => x.Employee, map =>{
                map.Column("EmployeeId");
                map.Lazy(LazyRelation.NoLazy);
                map.Class(typeof(Employee));
            });
            

        }
    }
}
