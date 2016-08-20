using System;
using System.Collections;
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
using AAS.ManagementClient.ViewModel;

namespace AAS.ManagementClient.Sections
{
    /// <summary>
    /// Interaction logic for EmployeeWorkSchedule.xaml
    /// </summary>
    public partial class EmployeeWorkSchedule : UserControl
    {
        public EmployeeWorkSchedule()
        {
            InitializeComponent();


            //Adding duration columns
            for (int col = 0; col < 24; col++)
            {
                DataGridTemplateColumn dataGridTemplateColumn = new DataGridTemplateColumn();
                FrameworkElementFactory checkBoxFactory = new FrameworkElementFactory(typeof(CheckBox));

                Binding binding = new Binding(string.Format("[{0}].Value", col))
                {
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
                checkBoxFactory.SetValue(CheckBox.IsCheckedProperty, binding);

                DataTemplate cellTemplate = new DataTemplate();
                cellTemplate.VisualTree = checkBoxFactory;
                dataGridTemplateColumn.CellTemplate = cellTemplate;

                dataGridTemplateColumn.Header = string.Format("{0}:00\n{1}:00", col.ToString().PadLeft(2, '0'), (col + 1).ToString().PadLeft(2, '0'));
                dataGrid.Columns.Add(dataGridTemplateColumn);



            }
            dataGrid.Columns[23].Header = "23:00\n00:00";

            Messenger.Default.Register<Messages>(this, (message) =>
            {
                BindingExpression bindingExpression = dataGrid.GetBindingExpression(DataGrid.ItemsSourceProperty);
                if (message == Messages.EmployeeRequestsInstance)
                {
                    if (bindingExpression != null)
                    {
                        bindingExpression.UpdateSource();
                        WorkSchedule boundWorkSchedule = (WorkSchedule)bindingExpression.DataItem;
                        Messenger.Default.Send<WorkSchedule, EmployeeViewModel>(boundWorkSchedule);
                    }
                }
                if (message == Messages.Reset)
                {
                    if (bindingExpression != null)
                    {
                        
                        ((WorkSchedule)bindingExpression.DataItem).Updated = true;

                        bindingExpression.UpdateSource();
                        bindingExpression.UpdateTarget();
                        WorkSchedule boundWorkSchedule = (WorkSchedule)bindingExpression.DataItem;
                        Messenger.Default.Send<WorkSchedule, EmployeeViewModel>(boundWorkSchedule);
                        ((WorkSchedule)bindingExpression.DataItem).Updated = false;
                    }
                }
            });
        }

        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

            e.Row.Header = ((string[])(Application.Current.Resources["DaysInWeek"]))[e.Row.GetIndex()];
        }

        private void dataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void dataGrid_MouseMove(object sender, MouseEventArgs e)
        {

        }

    }
}
