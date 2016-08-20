using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AAS.Entities;
using AAS.Entities.Exceptions;
using System.IO;
using AAS.Persistence.Mappings;
using System.Reflection;

namespace AAS.Persistence.Test.Unit
{
    [TestFixture(Category = "HeavyTests")]
    public class NHibernateHelperTest : AssertionHelper
    {
        #region Test Data
        List<Type> Types = new List<Type> { typeof(EmployeeMap), typeof(ContactInformationMap) };
        TestCaseData[] EmptyType = { new TestCaseData(null), new TestCaseData(new List<Type>()) };
        #endregion

        #region SessionFactory
        [Test]
        public void SessionFactory_WhenDatabaseWasNotInitialized_ThrowsNotInitializedException()
        {
            AAS.Test.Util.Mocker.MockPrivateStaticField(typeof(NHibernateHelper), "initialized", false, () =>
            {
                Expect(() => NHibernateHelper.SessionFactory, Throws.Exception.TypeOf<NotInitializedException>());
            });
        
        }
        [Test]
        public void SessionFactory_WhenDatabaseWasInitialized_Passes()
        {
            
            AAS.Test.Util.Mocker.MockPrivateStaticField(typeof(NHibernateHelper), "initialized", true, () =>
            {
                Expect(() => NHibernateHelper.SessionFactory, Throws.Nothing);
            });
        }
        #endregion

        #region InitializeDatabase
        [Test]
        public void InitializeDatabase_WhenCalled_Passes()
        {
            Expect(() => NHibernateHelper.InitializeDatabase("Memory", Types), Throws.Nothing);
        }
        [Test]
        public void InitializeDatabase_WithInvalidConnectionStringName_ThrowsKeyNotFoundException()
        {
            Expect(() => NHibernateHelper.InitializeDatabase("InvalidConnectionStringName", Types), Throws.Exception.TypeOf<KeyNotFoundException>());
        }
        [Test, TestCaseSource("EmptyType")]
        public void InitializeDatabase_WithNoTypes_CreatesDatabase_ArgumentNullException(List<Type> types)
        {
            Expect(() => NHibernateHelper.InitializeDatabase("Memory", types), Throws.Exception.TypeOf<ArgumentNullException>());
        }
        #endregion

        #region SetUp & TearDown
        [SetUp]
        public void SetUp()
        {
            NHibernateHelper.InitializeDatabase("Memory", Types);
        }
        [TearDown]
        public void TearDown()
        {
            AAS.Test.Util.Mocker.CallPrivateStaticMethod(typeof(NHibernateHelper), "reset", null);
        } 
        #endregion
    }
}
