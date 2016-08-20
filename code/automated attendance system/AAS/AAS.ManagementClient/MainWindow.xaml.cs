using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AAS.Entities;
using AAS.Entities.Exceptions;
using System.Globalization;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Reflection.Emit;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.ComponentModel;

using GalaSoft.MvvmLight.Messaging;

using AAS.ManagementClient.ViewModel;
using GalaSoft.MvvmLight.Command;
namespace AAS.ManagementClient
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        
        public MainWindow()
        {
            InitializeComponent();
            
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Add(new EmployeeView());
            //Messenger.Default.Send<Messages, EmployeeView>(Messages.ToggleEditMode);
        }

        private void dataGridEmployee_Selected(object sender, RoutedEventArgs e)
        {



        }

        private void dataGridEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (dataGridEmployee.SelectedItem != null)
            //{

            //    Messenger.Default.Send<Employee, EmployeeViewModel>((Employee)dataGridEmployee.SelectedItem);
            //    mainGrid.Children.Add(new EmployeeView());
            //}
        }
    }
}
