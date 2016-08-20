using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureRepository
{
    /// <summary>
    /// یک کلاس یرای ذخیره سازی و بازیابی تصاویر در یک فایل ZIP
    /// در واقع این کلاس به نوعی یک پایگاه داده برای تصاویر است
    /// </summary>
    public class PictureRepository
    {
        /// <summary>
        /// نام فایل ZIP
        /// </summary>
        private string m_dbFileName;

        /// <summary>
        /// متد سازنده
        /// </summary>
        /// <param name="dbfilename">مسیر فایل ZIP</param>
        public PictureRepository(string dbfilename)
        {
            m_dbFileName = dbfilename;
        }

        /// <summary>
        /// دخیره سازی یک تصویر
        /// </summary>
        /// <param name="fileName" >نام فایل</param>
        /// <param name="bitmap">تصویر</param>
        /// <returns>تصویر ذخیره شده را بر می گرداند</returns>
        public Bitmap Save(string fileName, Bitmap bitmap)
        {
            ///[Saving picture]
            using (ZipArchive archive = ZipFile.Open(m_dbFileName, ZipArchiveMode.Update))
            {

                ZipArchiveEntry entry = archive.GetEntry(fileName);
                if (entry == null)
                    entry = archive.CreateEntry(fileName);

                bitmap.Save(entry.Open(), ImageFormat.Jpeg);
            }
            ///[Saving picture]

            return bitmap;
        }

        /// <summary>
        /// بازیابی یک تصویر
        /// </summary>
        /// <param name="fileName">نام فایل مورد نظر</param>
        /// <returns>تصویر بازیابی شده را در صورت وجود بر می گرداند</returns>
        public Bitmap Read(string fileName)
        {
            ///[Reading picture]
            using (ZipArchive archive = ZipFile.Open(m_dbFileName, ZipArchiveMode.Read))
            {
                ZipArchiveEntry entry = archive.GetEntry(fileName);
                if (entry != null)
                {
                    return new Bitmap(entry.Open());
                }
            }
            return new Bitmap(1, 1);
            ///[Reading picture]
        }

        /// <summary>
        /// حذف تصویر
        /// </summary>
        /// <param name="fileName">نام فایل</param>
        /// <returns>موفقیت آمیز بودن عمل</returns>
        public bool Delete(string fileName)
        {
            ///[Deleting picture]
            using (ZipArchive archive = ZipFile.Open(m_dbFileName, ZipArchiveMode.Update))
            {
                ZipArchiveEntry entry = archive.GetEntry(fileName);
                if (entry != null)
                {

                    entry.Delete();
                }
                else return false;
            }
            return true;
            ///[Deleting picture]
        }

        /// <summary>
        /// یک دایرکتوری را حذف می کند.
        /// </summary>
        /// <param name="dirName">نام دایرکتوری</param>
        /// <returns>موفقیت آمیز بودن عمل</returns>
        public bool DeleteDirectory(string dirName)
        {
            ///[Deleting directory]
            using (ZipArchive archive = ZipFile.Open(m_dbFileName, ZipArchiveMode.Update))
            {
                IList<ZipArchiveEntry> entries = (archive.Entries.Where((e) => e.FullName.StartsWith(dirName + "\\") || e.FullName.StartsWith(dirName + "//"))).ToList();
                if (entries.Count == 0)
                    return false;
                for (int i = 0; i < entries.Count(); i++)
                {
                    entries[i].Delete();

                }
            }
            return true;
        }
        ///[Deleting directory]


    }
}
