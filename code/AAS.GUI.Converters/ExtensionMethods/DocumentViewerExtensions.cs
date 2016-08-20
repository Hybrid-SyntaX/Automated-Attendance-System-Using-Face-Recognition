using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace AAS.GUI.ExtensionMethods
{
    /// <summary>
    /// Extension Method های <c>DocumentViewer</c>
    /// این Method های برای رفع مشکل موجود در  Designer مربوط به XAML است، که در آن نمی توان به طور مستقیم یک صفحه را درون <c>FixedDocument</c> قرار داد.
    /// </summary>
    public static class DocumentViewerExtensions
    {
        /// <summary>
        /// بارگذاری و نمایش یک صفحه (<c>Page</c>) در <c>DocumentViewer</c>
        /// 
        /// </summary>
        /// <param name="documentViewer">نموهن جاری <c>DocumentViewer</c></param>
        /// <param name="page">صفحه مورد نظر</param>
        public static void LoadPage(this DocumentViewer documentViewer, Page page)
        {
            ///[Adding page to the pagecontent]
            PageContent pageContent = new PageContent();
            FixedPage fixedPage = (FixedPage)page.Content;
            page.Content = null;
            pageContent.Child = fixedPage;
            ///[Adding page to the pagecontent]
            
            ///[Creating fixedDocument and showing it in DocumentViewer]
            FixedDocument fixedDocument = new FixedDocument();

            fixedDocument.Pages.Add(pageContent);
            fixedDocument.DataContext = documentViewer.DataContext;
            page.DataContext = fixedDocument.DataContext;
            documentViewer.Document = fixedDocument;
            ///[Creating fixedDocument and showing it in DocumentViewer]
            
        }

        /// <summary>
        /// ایجاد یک صفحه از کنترل ورودی
        /// </summary>
        /// <param name="documentViewer">نموهن جاری <c>DocumentViewer</c></param>
        /// <param name="control">نمونه ای از کنترل مورد نظر</param>
        public static void GeneratePage(this DocumentViewer documentViewer, Control control)
        {
            ///[Generating page]
            PageContent pageContent = new PageContent();
            FixedPage fixedPage = new FixedPage();
            Panel originalParent = (control.Parent as Panel);
            originalParent.Children.Remove(control);


            fixedPage.Children.Add(control);
            pageContent.Child = fixedPage;

            FixedDocument fixedDocument = new FixedDocument();

            fixedDocument.Pages.Add(pageContent);
            fixedDocument.DataContext = documentViewer.DataContext;
            fixedPage.DataContext = documentViewer.DataContext;
            documentViewer.Document = fixedDocument;

            ///[Generating page]
        }
    }
}
