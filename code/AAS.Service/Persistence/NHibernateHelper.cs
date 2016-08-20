using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using AAS.Entities;
using AAS.Entities.Exceptions;
using AAS.Persistence.UserTypes;
using System.Data;
using NHibernate.Context;
namespace AAS.Persistence
{
    ///[In memory database workaround]
    /// <summary>
    /// کلاس برای ایجاد پایگاه داده در حافظه
    /// </summary>
    public class SqLiteInMemoryTestingConnectionProvider : NHibernate.Connection.DriverConnectionProvider
    {
        /// <summary>
        /// Connection
        /// </summary>
        public static System.Data.IDbConnection Connection = null;
        
        /// <summary>
        /// دریافت Connection
        /// </summary>
        public override System.Data.IDbConnection GetConnection()
        {
            if (Connection == null)
                Connection = base.GetConnection(); return Connection;
        }
        
        /// <summary>
        /// قطع ارتباط
        /// </summary>
        /// <param name="conn">Connection مورد نظر</param>
        public override void CloseConnection(System.Data.IDbConnection conn)
        {
        }

    }
    ///[In memory database workaround]
    
    /// <summary>
    /// کلاس کمکی برای استفاده از NHibernate
    /// </summary>
    public static class NHibernateHelper
    {
        private static Configuration m_configuration;
        private static ISessionFactory m_sessionFactory;
        private static List<Type> m_typeMaps;
        private static bool m_inMemory = false;
        private static bool initialized = false;

        /// <summary>
        /// SessionFactory
        /// </summary>
        public static ISessionFactory SessionFactory
        {
            get
            {
                if (!initialized)
                    throw new NotInitializedException("Was not initialized.\n you should call InitializedDatabase() method first");

                if (m_sessionFactory == null)
                    m_sessionFactory = getConfiguration().BuildSessionFactory();
                return m_sessionFactory;
            }
        }

        /// <summary>
        /// Session
        /// </summary>
        public static ISession Session
        {
            get
            {
                if(!CurrentSessionContext.HasBind(SessionFactory))
                    CurrentSessionContext.Bind(SessionFactory.OpenSession());

                return SessionFactory.GetCurrentSession();
            }
        }

        /// <summary>
        /// راه اندازی پایگاه داده مقیم در RAM
        /// </summary>
        /// <param name="typeMaps">Map ها</param>
        public static void InitializeInMemoryDatabase(List<Type> typeMaps)
        {
            ///[Initializing in-memory database]
            if (typeMaps == null || typeMaps.Count == 0)
                throw new ArgumentNullException("typeMaps was not provided");

            m_typeMaps = typeMaps;
            m_inMemory = true;

            new SchemaExport(getConfiguration()).Create(true, true);


            initialized = true;
            ///[Initializing in-memory database]
        }
        
        /// <summary>
        /// راه اندازی پایگاه داده
        /// </summary>
        /// <param name="connectionStringName">نام ConnetionString</param>
        /// <param name="typeMaps">Map ها</param>
        /// <param name="inMemory">در حافظه بسازد</param>
        public static void InitializeDatabase(string connectionStringName, List<Type> typeMaps, bool inMemory = false)
        {
            ///[Initializing database]
            m_connectionStringName = connectionStringName;

            if (getDataSource() == ":memory:")
                InitializeInMemoryDatabase(typeMaps);

            if (typeMaps == null || typeMaps.Count == 0)
                throw new ArgumentNullException("typeMaps was not provided");

            m_inMemory = inMemory;
            m_typeMaps = typeMaps;


            // IDbConnection connection = SessionFactory.OpenSession().Connection;

            if (!inMemory && System.IO.File.Exists(getDataSource()))
                new SchemaUpdate(getConfiguration()).Execute(true, true);
            else
                new SchemaExport(getConfiguration()).Create(true, true);
            //new SchemaExport(getConfiguration()).Execute(true, true, false, connection, null);


            initialized = true;
            ///[Initializing database]
        }

        /// <summary>
        /// دریافت تنظمیات
        /// </summary>
        /// <returns></returns>
        private static Configuration getConfiguration()
        {

            if (m_configuration == null)
                m_configuration = createConfiguration();

            return m_configuration;

        }

        /// <summary>
        /// تنظیمات
        /// </summary>
        public static Configuration Configuration
        {
            get
            {
                return getConfiguration();
            }
        }

        /// <summary>
        /// ایجاد تنظیامت
        /// </summary>
        /// <returns></returns>
        private static Configuration createConfiguration()
        {
            ///[Creating configuration]
            var configuration = new Configuration();
            //Loads nhibernate mappings 
            configuration.DataBaseIntegration(db =>
            {
                if (m_inMemory)
                {
                    db.ConnectionString = "Data Source=:memory:;Version=3;New=True";
                    db.ConnectionProvider<SqLiteInMemoryTestingConnectionProvider>();
                }
                else
                {
                    db.ConnectionStringName = m_connectionStringName;
                }
                db.Dialect<NHibernate.Dialect.SQLiteDialect>();
                db.BatchSize = 500;
                db.LogSqlInConsole = true;
                db.Driver<NHibernate.Driver.SQLite20Driver>();



            }).SessionFactory().GenerateStatistics();


            ModelMapper mapper = new ModelMapper();
            mapper.AddMappings(m_typeMaps);


            foreach (HbmMapping mapping in mapper.CompileMappingForEachExplicitlyAddedEntity())
            {
                configuration.AddMapping(mapping);
            }
            return configuration;
            ///[Creating configuration]
        }
        private static string m_connectionStringName;


        private static string getDataSource()
        {
            var conString = System.Configuration.ConfigurationManager.ConnectionStrings[m_connectionStringName];
            if (conString == null)
                throw new KeyNotFoundException(string.Format("No connection string named \" {0} \" was found.", m_connectionStringName));

            string dbName = conString.ConnectionString.Remove(conString.ConnectionString.IndexOf("Data Source="), "Data Source=".Length);
            if (dbName.Contains(';'))
                dbName = dbName.Remove(dbName.IndexOf(";"));

            return dbName;
        }


        private static void reset()
        {
            
            var export = new SchemaExport(m_configuration);
            export.Drop(true, true);


            m_typeMaps = null;
            m_connectionStringName = null;
            if (m_sessionFactory != null)
                m_sessionFactory.Dispose();
            m_sessionFactory = null;
            m_configuration = null;
            initialized = false;


        }
    }

}
