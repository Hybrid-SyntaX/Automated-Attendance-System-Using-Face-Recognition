using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PictureRepository;
using XMLKeyValueDatabase;
namespace AM.FRS
{
    /// <summary>
    /// نحوه ذخیره سازی
    /// </summary>
    public enum StorageMethod
    {
        /// <summary>
        /// دایرکتوری
        /// </summary>
        Directory,

        /// <summary>
        /// فایل ZIP
        /// </summary>
        ZipArchive
    }


    /// <summary>
    /// پایگاه داده صورت برای استفاده داخلی سیستم تشخیص چهره
    /// </summary>
    public static class FaceRepository
    {
        /// <summary>
        /// متد سازنده static
        /// </summary>
        static FaceRepository()
        {
            m_storageMethod = StorageMethod.ZipArchive;
            m_facePictureRepository = new PictureRepository.PictureRepository(@"faces\faces.zip");
        }
        private static XMLKeyValueDatabase.XMLKeyValueDatabase m_xmlKeyValueDatabase;
        private static PictureRepository.PictureRepository m_facePictureRepository;


        private static StorageMethod m_storageMethod;

        /// <summary>
        /// دخیره سازی تصویر صورت
        /// </summary>
        /// <param name="bitmap">تصویر</param>
        /// <param name="id">شناسه خارجی</param>
        public static void SaveFaceBitmap(Bitmap bitmap, string id)
        {
            ///[Saving face bitmap]
            int key = FaceRepository.getKey(id);

            if (m_storageMethod == StorageMethod.ZipArchive)
            {
                m_facePictureRepository.Save(string.Format(@"{0}\{1}.jpg", key, Guid.NewGuid()), bitmap);
            }

            ///[Saving face bitmap]
            else if (m_storageMethod == StorageMethod.Directory)
            {
                if (!Directory.Exists(string.Format(@"faces\{0}", key)))
                    Directory.CreateDirectory(string.Format(@"faces\{0}", key));

                bitmap.Save(string.Format(@"faces\{0}\{1}.jpg", key, Guid.NewGuid()), ImageFormat.Jpeg);
            }

            m_xmlKeyValueDatabase[Convert.ToString(key)] = id;

        }

        /// <summary>
        /// دریافت شناسه خارجی
        /// </summary>
        /// <param name="key">کلید</param>
        /// <returns>شناسه خارجی</returns>
        public static string GetId(int key)
        {
            return m_xmlKeyValueDatabase[key.ToString()];
        }

        /// <summary>
        /// دریافت کلید یا ایجاد کلید جدید
        /// </summary>
        /// <param name="id">شناسه خارجی</param>
        /// <returns>کلید</returns>
        private static int getKey(string id)
        {
            int key;
            if (m_xmlKeyValueDatabase.ContainsValue(id))
            {
                key = int.Parse(m_xmlKeyValueDatabase.RetrieveByValue(id).Key);
            }
            else
            {
                key = m_xmlKeyValueDatabase.Count + 1;
            }
            return key;
        }

        /// <summary>
        /// ساخت یا ذخیره پایگاه داده چهره
        /// </summary>
        /// <param name="images">تصاویر</param>
        /// <param name="labels">شناسه ها</param>
        public static void OpenFacesDatabase(ref List<Image<Gray, byte>> images, ref List<int> labels)
        {
            ///[Opening face database]
            if (m_storageMethod == StorageMethod.ZipArchive)
            {
                try
                {
                    ///[Create database if not exists]
                    if (!File.Exists(@"faces\faces.zip"))
                    {
                        using (ZipArchive archive = ZipFile.Open(@"faces\faces.zip", ZipArchiveMode.Create))
                        {
                            Stream stream = archive.CreateEntry("faces.xml").Open();
                            XMLKeyValueDatabase.XMLKeyValueDatabase.Create(ref stream);

                        }
                    }
                    ///[Create database if not exists]
                    if (File.Exists(@"faces\faces.zip"))
                    {
                        using (ZipArchive archive = ZipFile.OpenRead(@"faces\faces.zip"))
                        {

                            m_xmlKeyValueDatabase = new XMLKeyValueDatabase.XMLKeyValueDatabase(archive.GetEntry("faces.xml"), @"faces\faces.zip");

                            foreach (ZipArchiveEntry entry in archive.Entries)
                            {
                                if (!entry.FullName.Contains(".xml") && entry.Length > 0)
                                {


                                    Bitmap bitmap = null;
                                    string labelNumberStr = null;

                                    bitmap = new Bitmap(entry.Open());
                                    images.Add(new Image<Gray, byte>(bitmap));

                                    if (entry.FullName.Contains(@"/"))
                                        labelNumberStr = entry.FullName.Remove(entry.FullName.IndexOf(@"/"));
                                    else if (entry.FullName.Contains(@"\"))
                                        labelNumberStr = entry.FullName.Remove(entry.FullName.IndexOf(@"\"));
                                    else
                                        labelNumberStr = entry.FullName;



                                    labels.Add(int.Parse(labelNumberStr));
                                }

                            }
                        }
                    }
                    else
                    {



                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (m_storageMethod == StorageMethod.Directory)
            {
                m_xmlKeyValueDatabase = new XMLKeyValueDatabase.XMLKeyValueDatabase(@"faces\faces.xml");

                for (int i = 0; i <= m_xmlKeyValueDatabase.Count; i++)
                {
                    string l_dirname = string.Format(@"faces\{0}\", i + 1);
                    if (Directory.Exists(l_dirname))
                        foreach (string filename in Directory.GetFiles(l_dirname))
                        {
                            images.Add(new Image<Gray, byte>(filename));
                            labels.Add(i + 1);
                        }
                }
            }

            ///[Opening face database]
        }

        /// <summary>
        /// حذف صورت
        /// </summary>
        /// <param name="id">شناسه خارجی</param>
        public static void DeleteFace(string id)
        {
            ///[Deleting face]
            KeyValuePair<string, string> record = m_xmlKeyValueDatabase.RetrieveByValue(id);

            if (record.Key != null && record.Value != null)
            {

                if (m_facePictureRepository.DeleteDirectory(string.Format(@"{0}", record.Key)))
                    m_xmlKeyValueDatabase.Delete(record.Key);
            }
            ///[Deleting face]

        }

    }


}
