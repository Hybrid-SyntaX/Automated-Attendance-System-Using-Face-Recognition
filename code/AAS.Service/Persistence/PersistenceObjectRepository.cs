using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;


namespace AAS.Persistence
{
    /// <summary>
    /// اعمال ایجاد، بروزرسانی، بازیابی و حذف رکورد ها را انجام می دهد
    /// </summary>
    /// <typeparam name="T">نوع Map شده مورد نظر بری انجام اعمال ثبت، بازیابی، بروز رسانی و حذف</typeparam>
    public static class PersistenceObjectRepository<T> where T : class,Entities.IEntity
    {
        private static ISession m_currentSession;
        
        /// <summary>
        /// Session جاری
        /// </summary>
        private static ISession currentSession
        {
            get
            {
                if (m_currentSession == null)
                    m_currentSession = NHibernateHelper.SessionFactory.OpenSession();

                return m_currentSession;
            }
        }

        /// <summary>
        /// ثبت یا بروز رسانی رکورد
        /// </summary>
        /// <param name="po">نمونه</param>
        /// <returns>نمونه ثبت شده، یا بروز رسانی شده</returns>
        public static T CreateOrUpdate(T po)
        {
            using (ITransaction transaction = currentSession.BeginTransaction())
            {
                currentSession.SaveOrUpdate(po);
                transaction.Commit();
            }

            return po;
        }
        
        /// <summary>
        /// ایجاد یک رکورد جدید
        /// </summary>
        /// <param name="po">نمونه مورد نظر</param>
        /// <returns>نمونه ثبت شده</returns>
        public static T Create(T po)
        {
            //using (ISession session = NHibernateHelper.SessionFactory.OpenSession())
            using (ITransaction transaction = currentSession.BeginTransaction())
            {
                currentSession.Save(po);
                transaction.Commit();
            }

            return po;
        }

        /// <summary>
        /// ثبت لیستی از رکورد ها
        /// </summary>
        /// <param name="listOfPo">لیستی از رکورد ها</param>
        public static void Create(IList<T> listOfPo)
        {
            using (ITransaction transaction = currentSession.BeginTransaction())
            {
                foreach(T po in listOfPo)
                    currentSession.Save(po);

                transaction.Commit();
            }
        }

        /// <summary>
        /// بازیابی یک رکورد
        /// </summary>
        /// <param name="id">شناسه داخلی</param>
        /// <returns>نمونه ای از رکورد</returns>
        public static T Retrieve(Guid id)
        {
            ISession session = NHibernateHelper.SessionFactory.OpenSession();
            return session.QueryOver<T>().Where(x => x.ID == id).List().FirstOrDefault<T>();
        }

        /// <summary>
        /// بازیابی تمام کورد ها مربوط به نوع ورودی
        /// </summary>
        /// <returns>لیستی از تمام رکورد های مربوط به نوع انخاب شده</returns>
        public static List<T> RetrieveAll()
        {
            //currentSession = NHibernateHelper.SessionFactory.OpenSession();
            using (ISession session = NHibernateHelper.SessionFactory.OpenSession())
            {

                IQueryOver<T> query = session.QueryOver<T>(typeof(T).Name);

                return new List<T>(query.List<T>());
            }
        }

        /// <summary>
        /// بروز رسانی رکورد
        /// </summary>
        /// <param name="po"></param>
        /// <returns>نمونه به روز رسانی شده</returns>
        public static T Update(T po)
        {
            try
            {
                
                using (ISession session = NHibernateHelper.SessionFactory.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {

                    session.Update(po);
                    transaction.Commit();
                }
            }
            catch (Exception)
            {

                using (ITransaction transaction = currentSession.BeginTransaction())
                {

                    currentSession.Update(po);
                    transaction.Commit();
                }
            }
            return po;
        }

        /// <summary>
        /// حذف رکورد
        /// </summary>
        /// <param name="po">نمونه مورد نظر جهت حذف</param>
        public static void Delete(T po)
        {

            using (ISession session = NHibernateHelper.SessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(po);
                transaction.Commit();
            }
        }

        /// <summary>
        /// پاک کردن تمام رکورد ها
        /// </summary>
        public static void Truncate()
        {
            using (ISession session = NHibernateHelper.SessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.CreateQuery(string.Format("delete {0} f", typeof(T).Name)).ExecuteUpdate();
                    transaction.Commit();
                }

            }
        }
    }
}
