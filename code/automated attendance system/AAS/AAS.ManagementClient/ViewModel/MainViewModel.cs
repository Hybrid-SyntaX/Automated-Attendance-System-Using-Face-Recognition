using AAS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAS.Proxy;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using AAS.ManagementClient.View.Reports;
using System.Windows.Documents;
using AAS.GUI.ExtensionMethods;
using AAS.ManagementClient.Enums;
using System.Drawing;
using AM.QRService.Contracts;
using AM.FRS.Contracts;
using AAS.Service;
using AAS.Entities.Comparers;
using AAS.Entities.Interfaces;
namespace AAS.ManagementClient.ViewModel
{

    /// <summary>
    /// MainViewModel
    /// </summary>
    public class MainViewModel : AASViewModelBase
    {
        private bool m_filterMode;
        /// <summary>
        /// حالت فیلتر
        /// </summary>
        public bool FilterMode
        {
            set
            {
                m_filterMode = value;
                RaisePropertyChanged(() => FilterMode);
            }
            get
            {
                return m_filterMode;
            }

        }
        #region Services
        private IAutomatedAttendanceSystem AutomatedAttendanceSystem;
        private IQRService QRService;
        private IFaceRecognitionService FaceRecognitionService;
        #endregion

        #region UI
        private ContentControl m_content;
        /// <summary>
        /// قسمت نمایش صفحات برنامه
        /// </summary>
        public ContentControl Content
        {
            set
            {
                m_content = value;
                RaisePropertyChanged(() => Content);
            }
            get
            {
                return m_content;
            }
        }

        private Notification m_notificaiton;
        /// <summary>
        /// اعلان
        /// </summary>
        public Notification Notification
        {
            set
            {
                m_notificaiton = value;
                RaisePropertyChanged(() => this.Notification);
            }
            get
            {
                return m_notificaiton;
            }
        }
        #endregion

        #region Fields
        private ICollection<AttendanceTime> m_attendanceTimes;
        private ICollection<Employee> m_employees;

        private Storyboard fadeInStoryboard;
        private Storyboard fadeeOutStoryboard;
        #endregion

        #region Properties
        private ObservableCollection<Employee> m_filteredEmployees;
        /// <summary>
        /// لیست فیلتر شده کارمندان
        /// </summary>
        public ObservableCollection<Employee> FilteredEmployees
        {
            set
            {
                m_filteredEmployees = value;
                RaisePropertyChanged(() => FilteredEmployees);

            }
            get
            {
                return m_filteredEmployees;
            }
        }
        #endregion


        #region Constructor
        /// <summary>
        /// متن سازنده
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                FilteredEmployees = new ObservableCollection<Employee>()
                {
                    #region Abbas Allahyari
                    new Employee()
                    {
                        FirstName="عباس",
                        LastName="اللهیاری",
                        DateOfBirth=new DateTime(1991,02,09),
                        DateOfEmployement=new DateTime(2014,01,01),
                        EmployeeID=new EmployeeID(14,01,01),
                        NationalID = new IRNationalID("000-000004-3"),
                        IdentityNumber = "13654",
                        ProfilePicture = new Bitmap(@"G:\_DATA\dummy.png")
                    }, 
	                #endregion
                    #region Behnam Sajadi
                    new Employee()
                    {
                        FirstName="بهنام",
                        LastName="سجادی",
                        DateOfBirth=new DateTime(1994,02,09),
                        DateOfEmployement=new DateTime(2014,01,02),
                        EmployeeID=new EmployeeID(14,01,01),
                        NationalID = new IRNationalID("000-000004-3"),
                        IdentityNumber = "13654",
                        ProfilePicture = new Bitmap(@"G:\_DATA\dummy.png")
                    }, 
	                #endregion
                    #region Aryan Shams
                    new Employee()
                    {
                        FirstName="آرین",
                        LastName="شمس",
                        DateOfBirth=new DateTime(1994,02,09),
                        DateOfEmployement=new DateTime(2014,01,03),
                        EmployeeID=new EmployeeID(14,01,01),
                        NationalID = new IRNationalID("000-000004-3"),
                        IdentityNumber = "13654",
                        ProfilePicture = new Bitmap(@"G:\_DATA\dummy.png")
                    }, 
	                #endregion
                    #region Ami Sama
                    new Employee()
                    {
                        FirstName="آمی",
                        LastName="سَما",
                        DateOfBirth=new DateTime(1991,02,09),
                        DateOfEmployement=new DateTime(2014,01,04),
                        EmployeeID=new EmployeeID(14,01,01),
                        NationalID = new IRNationalID("000-000004-3"),
                        IdentityNumber = "13654",
                        ProfilePicture = new Bitmap(@"G:\_DATA\dummy.png")
                    }, 
	                #endregion
                };
                SelectedEmployeeFilter = (FilteredEmployees as ObservableCollection<Employee>)[0];
                RaisePropertyChanged(() => FilteredEmployees);
                Notification = new Notification()
                {
                    Message = "اعلان تست",
                    Type = NotificationType.Error,
                };
                FilterMode = true;

            }
            else
            {


                AutomatedAttendanceSystem = new AutomatedAttendanceSystemProxy();
                FaceRecognitionService = new FaceRecognitionServiceProxy();
                QRService = new QRServiceProxy();
                RetrieveEmployees();
                RetrieveAttendanceTimes();


                //CurrentAttendanceStatusFitler = AttendanceStatus.All;
                SearchQueryFilter = string.Empty;
                CurrentDateFilter = DateTime.Now;

                FilterMode = false;
                filterEmployees();
                Messenger.Default.Register<Notification>(this, (notification) =>
                {
                    //animation
                    Notification = notification;
                });
                fadeInStoryboard = (Storyboard)Application.Current.FindResource("fadeIn");
                fadeeOutStoryboard = (Storyboard)Application.Current.FindResource("fadeOut");
                Content = new ContentControl();
                Content.Visibility = Visibility.Collapsed;
                Content.Opacity = 0;
                Messenger.Default.Register<Messages>(this, (message) =>
                {
                    switch (message)
                    {
                        case Messages.Update:
                            RetrieveEmployees();

                            break;
                        case Messages.HideEmployeeView:
                            //animation

                            hide();
                            break;
                    }
                });
            }
        }

        private void RetrieveAttendanceTimes()
        {
            m_attendanceTimes = new List<AttendanceTime>(AutomatedAttendanceSystem.RetrieveAttendanceTimes());
        }
        #endregion
        private void show(Control page)
        {
            Content.Content = page;
            Content.BeginStoryboard(fadeInStoryboard, HandoffBehavior.Compose);
        }
        private void hide()
        {
            Content.BeginStoryboard(fadeeOutStoryboard, HandoffBehavior.Compose);
            Content.Content = null;
        }
        #region Methods

        private void RetrieveEmployees()
        {


            FilteredEmployees = new ObservableCollection<Employee>(m_employees = AutomatedAttendanceSystem.RetrieveEmployees());

            //  SelectedEmployeeFilter = null;
        }
        #endregion

        #region Filters
        private string m_searchQueryFilter;
        /// <summary>
        /// فیلتر جستجو
        /// </summary>
        public string SearchQueryFilter
        {
            set
            {
                m_searchQueryFilter = value;
                RaisePropertyChanged(() => SearchQueryFilter);
            }
            get
            {
                return m_searchQueryFilter;
            }
        }

        /// <summary>
        /// فیلتر وضعیت حضور
        /// </summary>
        public AttendanceStatus CurrentAttendanceStatusFitler { set; get; }

        /// <summary>
        /// فیلتر تاریخ
        /// </summary>
        public DateTime CurrentDateFilter { set; get; }

        private IEmployee m_selectedEmployee;
        /// <summary>
        /// فیلتر کارمند
        /// </summary>
        public IEmployee SelectedEmployeeFilter
        {
            set
            {
                m_selectedEmployee = value;
                RaisePropertyChanged(() => SelectedEmployeeFilter);
            }
            get
            {
                return m_selectedEmployee;
            }


        }


        #endregion

        #region Commands


        private ICommand m_FilterEmployeesCommand;

        /// <summary>
        /// اعمال فیلتر بر روی کارمندان
        /// </summary>
        public ICommand FilterEmployeesCommand
        {
            get
            {
                if (m_FilterEmployeesCommand == null)
                {
                    m_FilterEmployeesCommand = new RelayCommand(filterEmployees);
                }

                return m_FilterEmployeesCommand;
            }
        }
        private void filterEmployees()
        {
            IEnumerable<Employee> l_matchingEmployees = null;
            if (!FilterMode)
            {
                l_matchingEmployees = m_employees;

            }
            else
            {


                DateTime dateTime = CurrentDateFilter;


                IEnumerable<Employee> l_presentEmployees = null;
                IEnumerable<Employee> l_absentEmployees = null;

                if (CurrentDateFilter.Date <= DateTime.Now.Date)
                {
                    l_presentEmployees = (from at in m_attendanceTimes
                                          where AttendanceTime.IsPresent(at, CurrentDateFilter)
                                          select at.Employee).Distinct();


                    l_absentEmployees = m_employees.Except(l_presentEmployees, new EntityComparer<Employee>()).Distinct();
                }

                else
                {
                    l_presentEmployees = new List<Employee>();
                    l_absentEmployees = new List<Employee>();
                }


                switch (CurrentAttendanceStatusFitler)
                {

                    case AttendanceStatus.Present:
                        l_matchingEmployees = l_presentEmployees;

                        break;

                    case AttendanceStatus.Absent:
                        l_matchingEmployees = l_absentEmployees;

                        break;

                }
            }
            string text = SearchQueryFilter ?? string.Empty;
            if (l_matchingEmployees.Count() > 0)
                l_matchingEmployees = (from Employee employee in l_matchingEmployees
                                       where
                                       employee != null &&
                                       (

                                       employee.FirstName.ToLower().Contains(text.ToLower()) ||
                                       employee.LastName.ToLower().Contains(text.ToLower()) ||
                                       employee.EmployeeID.ToString().Contains(text) ||
                                       employee.NationalID.ToString().Contains(text) ||
                                       employee.IdentityNumber.ToString().Contains(text)
                                       )
                                       select employee).ToList();

            FilteredEmployees = new ObservableCollection<Employee>(l_matchingEmployees.ToList<Employee>());
        }

        private ICommand m_NewEmployeeCommand;
        /// <summary>
        /// کارمند جدید
        /// </summary>
        public ICommand NewEmployeeCommand
        {
            get
            {
                if (m_NewEmployeeCommand == null)
                {

                    m_NewEmployeeCommand = new RelayCommand(newEmployee);
                }
                return m_NewEmployeeCommand;
            }
        }
        private void newEmployee()
        {
            Messenger.Default.Send<Messages, EmployeeViewModel>(Messages.EnableEditMode);
            Messenger.Default.Send<Messages, EmployeeViewModel>(Messages.Reset);
            Messenger.Default.Send<Employee, EmployeeViewModel>(null);
            //Content = new EmployeeView();
            //Content.BeginStoryboard(fadeInStoryboard, HandoffBehavior.Compose);
            show(new EmployeeView());
        }

        private ICommand m_selectEmlpyeeCommmand;
        /// <summary>
        /// انتخاب کارمند
        /// </summary>
        public ICommand SelectEmployeeCommand
        {
            get
            {
                if (m_selectEmlpyeeCommmand == null)
                {
                    m_selectEmlpyeeCommmand = new RelayCommand(selectEmployee, () => SelectedEmployeeFilter != null);
                }
                return m_selectEmlpyeeCommmand;
            }
        }
        private void selectEmployee()
        {



            Messenger.Default.Send<IEmployee, EmployeeViewModel>(SelectedEmployeeFilter);
            Messenger.Default.Send<Messages, EmployeeViewModel>(Messages.DisableEditMode);
            Messenger.Default.Send<Employee, EmployeeAttendanceTimesViewModel>((Employee)SelectedEmployeeFilter);
            show(new EmployeeView());


        }


        private ICommand m_printCommand;
        /// <summary>
        /// چاپ
        /// </summary>
        public ICommand PrintCommand
        {
            get
            {
                if (m_printCommand == null)
                {
                    m_printCommand = new RelayCommand<DataGrid>(print);
                }
                return m_printCommand;

            }
        }
        private void print(DataGrid dataGrid)
        {
            DocumentViewer documentViewer = new DocumentViewer();
            documentViewer.DataContext = this;

            documentViewer.LoadPage(new FilteredGrid());
            documentViewer.Print();
        }

        private ICommand m_showEmployeeAttendanceTimesCommand;


        #endregion

    }
}
