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
using Microsoft.Win32;

namespace AAS.ManagementClient.Sections
{
    /// <summary>
    /// Interaction logic for EmployeePersonalInformation.xaml
    /// </summary>
    public partial class EmployeePersonalInformation : UserControl
    {
        public Dictionary<string, string> Data;
        public EmployeePersonalInformation()
        {
            InitializeComponent();
            
            //AutomatedAttendanceSystem.CreateEmployee
            //(
            //    textField_FirstName.Value,
            //    textField_LastnName.Value,
            //    textField_Gender.Value=="0"?Gender.Male:Gender.Female,
            //    DateTime.Parse(textField_DateOfBirth.Value),
            //    textField_IdentityNumber.Value,
            //    new IRNationalID(textField_NationalID.Value),
            //    new ProfilePicture(@"K:\dummy.png"),
            //    null
            //);

            //this.Visibility = System.Windows.Visibility.Hidden;
        }
        private void toggleVisiblity()
        {
            //if (stackPersonalInformation.Visibility == System.Windows.Visibility.Visible)
            //{

            //    stackPersonalInformation.Visibility = System.Windows.Visibility.Hidden;
            //    image_ProfilePicture.Visibility = System.Windows.Visibility.Hidden;
            //}
            //else
            //{
            //    stackPersonalInformation.Visibility = System.Windows.Visibility.Visible;
            //    image_ProfilePicture.Visibility = System.Windows.Visibility.Visible;
            //}

        }
        private void textBlock_Title_MouseDown(object sender, MouseButtonEventArgs e)
        {
         //   toggleVisiblity();
        }
    }
}
