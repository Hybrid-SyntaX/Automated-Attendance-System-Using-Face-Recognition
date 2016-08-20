using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AAS.ManagementClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
           // CultureInfo persianCultureInfo = new CultureInfo("fa-IR");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("fa-IR");
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("fa-IR");


        }
    }
}
