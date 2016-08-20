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
using GalaSoft.MvvmLight.Messaging;
using AAS.ManagementClient.Enums;
namespace AAS.ManagementClient
{
    /// <summary>
    /// Interaction logic for AddEmployeeControl.xaml
    /// </summary>
    public partial class EmployeeView : UserControl
    {
        public EmployeeView()
        {
            InitializeComponent();
        }

        private void tabControl_ContactInformations_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void tabControl_ContactInformations_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void tabControl_ContactInformations_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

        }



    }
}
