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

namespace AAS.ManagementClient.Sections
{
    /// <summary>
    /// Interaction logic for EmployeeContactInformation.xaml
    /// </summary>
    public partial class EmployeeContactInformation : UserControl
    {
        public EmployeeContactInformation()
        {
            
            InitializeComponent();
        }

        
        private void textBlock_Title_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void textBlock_Title_MouseDown_1(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //ItemCollection tabs=tabControl_ContactInformations.Items;
            //TabItem firstTab = (TabItem)tabs[0];
            //TabItem tab = new TabItem();

            ////tab.SetBinding(TabItem.HeaderProperty,new Binding(firstTab.Header));
            //tab.Content = firstTab.Content;
            //tabs.Add(tab);
        }
    }
}
