using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XMLKeyValueDatabase
{
    /// <summary>
    /// یک پایگاه داده Key/Value پیاده سازی شده با استفاده از XML
    /// </summary>
    public class XMLKeyValueDatabase
    {
        /// <summary>
        /// سند XML
        /// </summary>
        private XDocument m_document;

        /// <summary>
        /// نام فایل XML
        /// </summary>
        private string m_fileName;

        /// <summary>
        /// نام فایل ZIP
        /// </summary>
        private string m_archivePath;

        /// <summary>
        /// <c>Stream</c> در حالت بازکردن با ZIP
        /// </summary>
        private Stream m_stream;

        /// <summary>
        /// مدخل فایل XML در فایل ZIP
        /// </summary>
        private ZipArchiveEntry m_zipEntry;

        /// <summary>
        /// برسی اینکه آیا <c>XMLKeyValueDatabase</c> با <c>Stream</c> مقدار دهی شده است.
        /// </summary>
        private bool m_isStream = false;

        /// <summary>
        /// بافر برای ذخیره سازی سند در مدخل ZIP
        /// </summary>
        private byte[] m_buffer;

        /// <summary>
        /// لیست تمامی کلید ها
        /// </summary>
        public List<string> Keys
        {
            get
            {
                return (from kvp in m_document.Root.Descendants("entry").Attributes("key") select kvp.Value).ToList<string>();
            }
        }

        /// <summary>
        /// لیست تمام مقادیر
        /// </summary>
        public List<string> Values
        {
            get
            {

                return (from kvp in m_document.Root.Descendants("entry").Attributes("value") select kvp.Value).ToList<string>();
            }
        }

        /// <summary>
        /// indexer برای <c>XMLKeyValueDatabase</c>
        /// </summary>
        /// <param name="key">کلید</param>
        /// <returns>مقدار متناظر با کلید</returns>
        public string this[string key]
        {
            get 
            { 
                return retrieve(key).Value; 
            }
            set 
            { 
                createOrUpdate(key, value); 
            }
        }

        /// <summary>
        /// تعداد رکورد های موجود
        /// </summary>
        public int Count
        {
            get
            {

                return m_document.Root.Descendants().Count();
            }
        }

        /// <summary>
        /// ایجاد یا بروزرسانی رکورد در دیتابیس
        /// </summary>
        /// <param name="key">کلید</param>
        /// <param name="value">مقدار</param>
        /// <returns>یک <c>KeyValuePair<string,string></c> حاوی کلید و مقدار</returns>
        private KeyValuePair<string, string> createOrUpdate(string key, string value)
        {
            ///[Creating or updating]
            if (retrieveXMLElement(key) == null)
            {
                XElement keyValuePairElement = new XElement("entry",
                  new XAttribute("key", key),
                  new XAttribute("value", value));

                m_document.Root.Add(keyValuePairElement);
            }
            else
            {
                XElement keyValuePairElement = retrieveXMLElement(key);
                keyValuePairElement.Attribute("value").SetValue(value);
            }
            try
            {
                submitChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return new KeyValuePair<string, string>(key, value);
            ///[Creating or updating]
        }
      
        /// <summary>
        /// بازیابی با استفاده از کلید
        /// </summary>
        /// <param name="key">کلید</param>
        /// <returns>یک <c>KeyValuePair<string,string></c> حاوی کلید و مقدار</returns>
        private KeyValuePair<string, string> retrieve(string key)
        {
            ///[Retrieving]
            XElement found = retrieveXMLElement(key);
            if (found == null)
                return new KeyValuePair<string, string>(null, null);
            return new KeyValuePair<string, string>(found.Attribute("key").Value, found.Attribute("value").Value);
            ///[Retrieving]
        }
        
        /// <summary>
        /// بازیابی المان حاوی کلید در فایل XML
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private XElement retrieveXMLElement(string key)
        {
            XElement found = m_document.Root.Descendants("entry").FirstOrDefault((x) => x.Attribute("key").Value == key);
            return found;
        }
        
        /// <summary>
        /// ثبت تغییرات در پایگاه داده
        /// </summary>
        private void submitChanges()
        {
            if (m_isStream)
            {
                ///[m_document is written into memory stream]
                MemoryStream memoryStream = new MemoryStream();
                m_document.Save(memoryStream);

                memoryStream.Position = 0;
                m_buffer = new byte[memoryStream.Length];

                memoryStream.Read(m_buffer, 0, m_buffer.Length);
                ///[m_document is written into memory stream]
             
                ///[Saving buffer into zip entry]
                using (ZipArchive archive = ZipFile.Open(m_archivePath, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry entry = archive.GetEntry(m_zipEntry.FullName);
                    entry.Delete();

                    Stream stream = archive.CreateEntry(m_zipEntry.FullName).Open();
                    stream.Write(m_buffer, 0, m_buffer.Length);
                    stream.Close();
                }
                ///[Saving buffer into zip entry]
            }
            else
                m_document.Save(m_fileName);
        }

        /// <summary>
        /// ساخت یک پایگاه داده
        /// </summary>
        /// <param name="stream"><c>Stream</c> فایل مورد نظر</param>
        public static void Create(ref Stream stream)
        {
            XDocument xdocument = new XDocument(new XElement("root"));

            xdocument.Save(stream);
        }
       
        /// <summary>
        /// بازیابی با استفاده از مقدار
        /// </summary>
        /// <param name="value">مقدار</param>
        /// <returns>یک <c>KeyValuePair<string,string></c> حاوی کلید و مقدار</returns>
        public KeyValuePair<string, string> RetrieveByValue(string value)
        {
            ///[Retrieve by value]
            XElement found = m_document.Root.Descendants("entry").FirstOrDefault((x) => x.Attribute("value").Value == value);
            if (found == null)
                return new KeyValuePair<string, string>(null, null);
            else return new KeyValuePair<string, string>(found.Attribute("key").Value, found.Attribute("value").Value);
            ///[Retrieve by value]
        }

        /// <summary>
        /// برسی وجود کلید
        /// </summary>
        /// <param name="key">کلید</param>
        /// <returns>نتیجه برسی</returns>
        /// <seealso cref="ContainsValue"/>
        public bool ContainsKey(string key)
        {
            return Keys.Contains(key);
        }
        
        /// <summary>
        /// برسی وجود مقدار
        /// </summary>
        /// <param name="value">مقدار</param>
        /// <returns>نتیجه برسی</returns>
        /// <seealso cref="ContainsKey"/>
        public bool ContainsValue(string value)
        {
            return Values.Contains(value);
        }
        

        /// <summary>
        /// متد سازنده <c>XMLKeyValueDatabase</c> برای فایل XML
        /// </summary>
        /// <param name="fileName">مسیر فایل XML</param>
        public XMLKeyValueDatabase(string fileName)
        {
            m_fileName = fileName;
            if (File.Exists(m_fileName))
                m_document = XDocument.Load(m_fileName);
            else
                m_document = new XDocument(new XElement("root"));
        }    
        
        /// <summary>
        /// متد سازنده <c>XMLKeyValueDatabase</c> برای فایل XML درون فایل ZIP
        /// </summary>
        /// <param name="zipEntry">مدخل مربوط به فایل XML در فایل ZIP</param>
        /// <param name="archivePath">مسیر فایل ZIP</param>
        public XMLKeyValueDatabase(ZipArchiveEntry zipEntry, string archivePath)
        {
            m_archivePath = archivePath;
            m_zipEntry = zipEntry;
            m_stream = zipEntry.Open();
            m_isStream = true;


            m_document = XDocument.Load(m_stream);

        }

        /// <summary>
        /// حذف رکورد
        /// </summary>
        /// <param name="key">مقدار کلید</param>
        /// <returns>موفقیت آمیز بودن عمل</returns>
        public bool Delete(string key)
        {
            ///[Deleting record]
            var record = retrieveXMLElement(key);
            if (record != null)
            {
                record.Remove();
                submitChanges();
                return true;
            }
            return false;
            ///[Deleting record]
        }

    }
}
