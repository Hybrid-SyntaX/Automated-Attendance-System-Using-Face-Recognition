using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAS.Entities;
using GalaSoft.MvvmLight;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Windows.Data;
using Microsoft.Win32;
using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel;
using System.Linq.Expressions;
using AAS.Entities.Exceptions;
using AAS.ManagementClient.Languages;
using AAS.Service;
using AAS.Proxy;
using System.Windows.Controls;
using AAS.ManagementClient.View.Reports;
using System.Drawing;
using AM.QRService.Contracts;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AAS.Entities.Interfaces;
using System.Collections.ObjectModel;
using AAS.ManagementClient.Enums;
using AAS.ManagementClient.Properties;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using System.Collections;
using System.Data;
using AAS.ManagementClient.Sections;
using AAS.GUI.ExtensionMethods;
using AM.FRS.Contracts;
using GalaSoft.MvvmLight.Ioc;
namespace AAS.ManagementClient.ViewModel
{
    /// <summary>
    /// EmployeeViewModel
    /// </summary>
    public class EmployeeViewModel : AASViewModelBase, IEmployee
    {
        #region Services
        private IAutomatedAttendanceSystem AutomatedAttendanceSystem;
        private IFaceRecognitionService FaceRecognitionService;
        private IQRService QRService;
        #endregion

        #region EmployeeViewModel Fields
        private Process m_FRSManagementClient;
        private GregorianCalendar m_greogianCalendar = new GregorianCalendar();
        #endregion

        #region EmployeeViewModel Properties
        private Employee m_currentEmployee;
        /// <summary>
        /// کارمند جاری
        /// </summary>
        private Employee CurrentEmployee
        {
            set
            {
                m_currentEmployee = value;
                RaisePropertyChanged(() => CurrentEmployee);
            }
            get
            {


                //TOOD: FIX THIS
                if (m_currentEmployee == null)
                    Messenger.Default.Register<IEmployee>(this, (message) =>
                    {
                        if (message is Employee)
                            m_currentEmployee = (Employee)message;
                        else if (message is EmployeeViewModel)
                            m_currentEmployee = ((EmployeeViewModel)message).CurrentEmployee;
                    });
                if (m_currentEmployee == null)
                {
                    m_currentEmployee = new Employee();
                }


                return m_currentEmployee;
            }
        }

        private bool editMode;
        
        /// <summary>
        /// حالت ویرایش
        /// </summary>
        public bool EditMode
        {
            set
            {
                editMode = value;
                RaisePropertyChanged(() => this.EditMode);
            }
            get
            {


                return editMode;
            }
        }

        private object m_content;
        /// <summary>
        /// مکان نمایش کارت بر روی صفحه
        /// </summary>
        public object Content
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
        #endregion

        #region Employee Members

        public EmployeeID EmployeeID
        {
            get
            {
                return CurrentEmployee.EmployeeID;
            }
            set
            {
                CurrentEmployee.EmployeeID = value;
                RaisePropertyChanged(() => this.EmployeeID);
            }
        }

        public DateTime DateOfEmployement
        {
            get
            {
                return CurrentEmployee.DateOfEmployement;
            }
            set
            {
                CurrentEmployee.DateOfEmployement = value;
                RaisePropertyChanged(() => DateOfEmployement);
            }
        }

        public string FirstName
        {
            set
            {
                try
                {
                    CurrentEmployee.FirstName = value;
                    RemoveError(() => FirstName);
                }
                catch (RequiredFieldException)
                {
                    AddError(() => FirstName, string.Format(fa_IR.RequiredField, getFieldLabel(() => this.FirstName)));
                }
                catch (InvalidFormatException)
                {
                    AddError(() => FirstName, string.Format(fa_IR.InvalidName, getFieldLabel(() => this.FirstName)));
                }
                catch (Exception ex)
                {
                    //TODO: log FirstName
                    AddError(() => FirstName, string.Format(fa_IR.UnknownError, getFieldLabel(() => this.FirstName)));
                }
                finally
                {
                    RaisePropertyChanged(() => this.FirstName);
                    RaiseErrorsChanged(() => this.FirstName);
                }
            }
            get
            {
                return CurrentEmployee.FirstName;
            }
        }
        public string LastName
        {
            set
            {
                try
                {
                    CurrentEmployee.LastName = value;
                    RemoveError(() => LastName);
                }
                catch (RequiredFieldException)
                {
                    AddError(() => LastName, string.Format(fa_IR.RequiredField, getFieldLabel(() => this.LastName)));
                }
                catch (InvalidFormatException)
                {
                    AddError(() => LastName, string.Format(fa_IR.InvalidName, getFieldLabel(() => this.LastName)));
                }
                catch (Exception ex)
                {
                    //TOOD: log here
                    AddError(() => LastName, string.Format(fa_IR.UnknownError, getFieldLabel(() => this.LastName)));
                }
                finally
                {
                    RaisePropertyChanged(() => this.LastName);
                    RaiseErrorsChanged(() => this.LastName);
                }
            }
            get
            {
                return CurrentEmployee.LastName;
            }
        }
        public string IdentityNumber
        {
            set
            {
                try
                {
                    CurrentEmployee.IdentityNumber = value;
                    RemoveError(() => IdentityNumber);

                }
                catch (RequiredFieldException)
                {
                    AddError(() => IdentityNumber, string.Format(fa_IR.RequiredField, getFieldLabel(() => this.IdentityNumber)));
                }
                catch (InvalidIdentityNumberException)
                {
                    AddError(() => IdentityNumber, string.Format(fa_IR.InvalidNumber, getFieldLabel(() => this.IdentityNumber)));
                }
                catch (Exception ex)
                {
                    //TOOD: log here
                    AddError(() => IdentityNumber, string.Format(fa_IR.UnknownError, getFieldLabel(() => this.IdentityNumber)));
                }
                finally
                {

                    RaisePropertyChanged(() => this.IdentityNumber);
                    RaiseErrorsChanged(() => this.IdentityNumber);
                }
            }
            get
            {
                return CurrentEmployee.IdentityNumber;
            }
        }
        public Gender Gender
        {
            set
            {
                try
                {
                    CurrentEmployee.Gender = value;
                    RemoveError(() => Gender);
                }
                catch (Exception ex)
                {
                    AddError(() => Gender, string.Format(fa_IR.UnknownError, getFieldLabel(() => Gender)));
                }
                finally
                {
                    RaisePropertyChanged(() => this.Gender);
                    RaiseErrorsChanged(() => Gender);
                }
            }
            get
            {
                return CurrentEmployee.Gender;
            }
        }
        public IRNationalID NationalID
        {
            set
            {
                try
                {
                    CurrentEmployee.NationalID = value;
                    RemoveError(() => NationalID);
                }
                catch (ArgumentNullException)
                {
                    AddError(() => NationalID, string.Format(fa_IR.RequiredField, getFieldLabel(() => this.NationalID)));
                }
                catch (RequiredFieldException)
                {
                    AddError(() => NationalID, string.Format(fa_IR.RequiredField, getFieldLabel(() => this.NationalID)));
                }
                catch (InvalidIRNationalIDFormatException)
                {
                    AddError(() => NationalID, string.Format(fa_IR.InvalidNationalIDFormat));
                }
                catch (InvalidIRNationalIDException)
                {
                    AddError(() => NationalID, string.Format(fa_IR.IsInvalid, getFieldLabel(() => this.NationalID)));
                }
                catch (Exception)
                {
                    AddError(() => NationalID, string.Format(fa_IR.UnknownError, getFieldLabel(() => NationalID)));
                }
                finally
                {
                    RaisePropertyChanged(() => this.NationalID);
                    RaiseErrorsChanged(() => this.NationalID);

                }

            }
            get
            {
                return CurrentEmployee.NationalID;
            }
        }
        public DateTime DateOfBirth
        {
            set
            {
                try
                {

                    //GregorianCalendar gregorianCalendar = new GregorianCalendar();


                    CurrentEmployee.DateOfBirth = new DateTime(m_greogianCalendar.GetYear(value), m_greogianCalendar.GetMonth(value), m_greogianCalendar.GetDayOfMonth(value));
                    RemoveError(() => this.DateOfBirth);
                }
                catch (RequiredFieldException)
                {
                    AddError(() => DateOfBirth, string.Format(fa_IR.RequiredField, getFieldLabel(() => DateOfBirth)));
                }
                catch (InvalidDateException)
                {
                    AddError(() => DateOfBirth, string.Format(fa_IR.IsInvalid, getFieldLabel(() => DateOfBirth)));
                }
                catch (IllegalAgeException)
                {
                    AddError(() => DateOfBirth, string.Format(fa_IR.OlderThanEighteen, getFieldLabel(() => DateOfBirth)));
                }
                catch (InvalidFormatException)
                {
                    AddError(() => DateOfBirth, string.Format(fa_IR.InvalidDateFormat, getFieldLabel(() => DateOfBirth)));
                }
                catch (Exception)
                {
                    //log
                    AddError(() => DateOfBirth, string.Format(fa_IR.UnknownError, getFieldLabel(() => DateOfBirth)));
                }
                finally
                {
                    RaisePropertyChanged(() => this.DateOfBirth);
                    RaiseErrorsChanged(() => this.DateOfBirth);
                }
            }
            get
            {
                return CurrentEmployee.DateOfBirth;
            }
        }
        public Bitmap ProfilePicture
        {
            set
            {
                try
                {


                    CurrentEmployee.ProfilePicture = value;
                    RemoveError(() => this.ProfilePicture);

                }
                catch (RequiredFieldException)
                {
                    AddError(() => ProfilePicture, string.Format(fa_IR.RequiredField, getFieldLabel(() => ProfilePicture)));
                }
                catch (Exception)
                {
                    AddError(() => ProfilePicture, string.Format(fa_IR.UnknownError, getFieldLabel(() => ProfilePicture)));
                }
                finally
                {
                    RaisePropertyChanged(() => this.ProfilePicture);
                    RaiseErrorsChanged(() => this.ProfilePicture);

                }
            }
            get
            {

                return CurrentEmployee.ProfilePicture;
            }
        }

        public WorkSchedule WorkSchedule
        {
            set
            {
                CurrentEmployee.WorkSchedule = value;
                RaisePropertyChanged(() => WorkSchedule);
            }
            get
            {

                return CurrentEmployee.WorkSchedule;

            }
        }

        public IList<ContactInformation> ContactInformations
        {
            set
            {
                throw new NotImplementedException();
            }
            get
            {
                //ContactInformations.Add(new ContactInformationViewModel());
                return new ObservableCollection<ContactInformation>(CurrentEmployee.ContactInformations);

            }
        }



        public IList<AttendanceTime> AttendanceTimes
        {
            get
            {
                return CurrentEmployee.AttendanceTimes;
            }
            set
            {

                throw new NotSupportedException();
            }
        }

        public Guid ID
        {
            get
            {
                return CurrentEmployee.ID;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public ICollection<ContactInformationViewModel> ContactInformationVideModels
        {
            get
            {

                ObservableCollection<ContactInformationViewModel> l_viewModels = new ObservableCollection<ContactInformationViewModel>();
                foreach (ContactInformation ci in CurrentEmployee.ContactInformations)
                {

                    l_viewModels.Add(new ContactInformationViewModel(ci) { EditMode = this.EditMode });
                }

                return l_viewModels;
            }
        }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }
        #endregion

        #region QR Card
        /// <summary>
        /// کد QR
        /// </summary>
        public Bitmap QRCode
        {

            get
            {
                return QRService.Write(CurrentEmployee.ID.ToString());
            }
        }
        #endregion

        #region Constructors & Descturoctors
        /// <summary>
        /// متد سازنده
        /// </summary>
        /// <param name="employee">کارمند</param>
        public EmployeeViewModel(Employee employee)
        {
            CurrentEmployee = employee;
            initialize();
        }

        /// <summary>
        /// متد سازنده
        /// </summary>
        [PreferredConstructor]
        public EmployeeViewModel()
        {
            if (IsInDesignMode)
            {
                m_currentEmployee = new Employee()
                {
                    FirstName = "عباس",
                    LastName = "اللهیاری",
                    DateOfBirth = new DateTime(1991, 02, 09),
                    DateOfEmployement = new DateTime(2014, 01, 01),
                    EmployeeID = new EmployeeID(14, 01, 01),
                    NationalID = new IRNationalID("000-000004-3"),
                    IdentityNumber = "13654",
                    Gender = Gender.Male,
                    // ProfilePicture = new Bitmap(@"G:\_DATA\dummy.png"),
                    WorkSchedule = WorkSchedule.MinValue
                };
                m_currentEmployee.WorkSchedule.DefineRange(DayOfWeek.Saturday, 9, 17, Entities.WorkSchedule.State.Work);
                m_currentEmployee.WorkSchedule.DefineRange(DayOfWeek.Sunday, 9, 17, Entities.WorkSchedule.State.Work);
                m_currentEmployee.WorkSchedule.DefineRange(DayOfWeek.Monday, 0, 23, Entities.WorkSchedule.State.Work);
                m_currentEmployee.WorkSchedule.DefineRange(DayOfWeek.Tuesday, 9, 17, Entities.WorkSchedule.State.Work);
                m_currentEmployee.WorkSchedule.DefineRange(DayOfWeek.Wednesday, 5, 23, Entities.WorkSchedule.State.Work);
                m_currentEmployee.WorkSchedule.DefineRange(DayOfWeek.Thursday, 9, 21, Entities.WorkSchedule.State.Work);
                m_currentEmployee.WorkSchedule.DefineRange(DayOfWeek.Friday, 9, 17, Entities.WorkSchedule.State.Off);

                //Messenger.Default.Send<WorkSchedule, WorkScheduleViewModel>(WorkSchedule);
                RaisePropertyChanged(() => CurrentEmployee);
                RaisePropertyChanged(() => WorkSchedule);


            }
            else
            {
                initialize();
            }

        }

        private void initialize()
        {
            //Initalize services
            this.AutomatedAttendanceSystem = new AutomatedAttendanceSystemProxy();
            this.FaceRecognitionService = new FaceRecognitionServiceProxy();
            this.QRService = new QRServiceProxy();

            //Register for messages
            Messenger.Default.Register<WorkSchedule>(this, (workScheduleMessage) =>
            {
                if (workScheduleMessage != WorkSchedule.MinValue)
                    WorkSchedule = workScheduleMessage;
            });
            Messenger.Default.Register<Messages>(this, (msg) =>
            {
                if (msg == Messages.ToggleEditMode)
                    toggleEditMode();
                if (msg == Messages.EnableEditMode)
                    toggleEditMode(true);
                if (msg == Messages.DisableEditMode)
                    toggleEditMode(false);
            });


          //  if (HasRecord)
                Messenger.Default.Send<Employee, EmployeeAttendanceTimesViewModel>(CurrentEmployee);
        }
        /// <summary>
        /// متد تخریب گر
        /// </summary>
        ~EmployeeViewModel()
        {
            if (m_FRSManagementClient != null && m_FRSManagementClient.HasExited == false)
                m_FRSManagementClient.CloseMainWindow();
        }
        #endregion

        #region Methods
        private void revertInstance()
        {
            if (EditMode == true)
            {
                CurrentEmployee = AutomatedAttendanceSystem.RetrieveEmployee(CurrentEmployee.ID);
                referesh();
            }
            toggleEditMode();
        }
        private void toggleEditMode()
        {

            EditMode = !EditMode;
            Messenger.Default.Send<Messages, ContactInformationViewModel>(Messages.ToggleEditMode);
            //Messenger.Default.Send<Messages, WorkScheduleViewModel>(Messages.ToggleEditMode);
        }
        private void toggleEditMode(bool editMode)
        {
            EditMode = editMode;
            if (editMode)
            {

                Messenger.Default.Send<Messages, ContactInformationViewModel>(Messages.EnableEditMode);
                //  Messenger.Default.Send<Messages, WorkScheduleViewModel>(Messages.EnableEditMode);
            }
            else
            {
                Messenger.Default.Send<Messages, ContactInformationViewModel>(Messages.DisableEditMode);
                //   Messenger.Default.Send<Messages, WorkScheduleViewModel>(Messages.DisableEditMode);
            }
        }
        #endregion

        #region Commands
        private ICommand m_setProfilePictureCommand;
        /// <summary>
        /// انتخاب عکس پرسنلی
        /// </summary>
        public ICommand SetProfilePictureCommand
        {
            get
            {
                if (m_setProfilePictureCommand == null)
                    m_setProfilePictureCommand = new RelayCommand(() =>
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.ShowDialog();
                        if (!string.IsNullOrEmpty(openFileDialog.FileName))
                            ProfilePicture = new Bitmap(openFileDialog.FileName);
                        else
                        {
                            AddError(() => ProfilePicture, string.Format(fa_IR.RequiredField, getFieldLabel(() => ProfilePicture)));
                            RaiseErrorsChanged(() => ProfilePicture);
                        }

                    });

                return m_setProfilePictureCommand;
            }
        }

        private ICommand m_saveCommand;
        /// <summary>
        /// ذخیره
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                if (m_saveCommand == null)
                    m_saveCommand = new RelayCommand(save, () => !HasErrors && HasRequiredField);

                return m_saveCommand;
            }

        }
        private void save()
        {
            try
            {
                Messenger.Default.Send<Messages, EmployeeWorkSchedule>(Messages.EmployeeRequestsInstance);
                Messenger.Default.Register<WorkSchedule>(this, (workschedule) => WorkSchedule = workschedule);
                if (CurrentEmployee.ID == Guid.Empty)   //new employee
                {
                    DateOfEmployement = new DateTime(m_greogianCalendar.GetYear(DateTime.Now),
                      m_greogianCalendar.GetMonth(DateTime.Now),
                     m_greogianCalendar.GetDayOfMonth(DateTime.Now),
                      m_greogianCalendar);

                    EmployeeID = AutomatedAttendanceSystem.NewEmployeeID();

                    CurrentEmployee = AutomatedAttendanceSystem.CreateEmployee(CurrentEmployee);
                    Messenger.Default.Send<Notification, MainViewModel>(new Notification()
                    {
                        Message = string.Format(fa_IR.EmployeeHasBeenCreatedSuccessfully, CurrentEmployee.FullName, CurrentEmployee.EmployeeID.ToString()),
                        Type = NotificationType.Info
                    });

                }
                else //Edit mode
                {
                    CurrentEmployee = AutomatedAttendanceSystem.UpdateEmployee(CurrentEmployee);
                    Messenger.Default.Send<Notification, MainViewModel>(new Notification()
                    {
                        Message = string.Format(fa_IR.EmployeeHasBeenUpdatedSuccessfully, CurrentEmployee.FullName, CurrentEmployee.EmployeeID.ToString()),
                        Type = NotificationType.Info
                    });
                }
                Messenger.Default.Send<Messages, MainViewModel>(Messages.Update);
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<Notification, MainViewModel>(new Notification()
                {
                    Message = ex.Message,
                    Type = NotificationType.Error
                });
            }
            toggleEditMode(false);
            RaisePropertyChanged(() => HasRecord);
        }

        private ICommand m_resetCommand;
        /// <summary>
        /// ریست
        /// </summary>
        public ICommand ResetCommand
        {
            get
            {
                if (m_resetCommand == null)
                    m_resetCommand = new RelayCommand(reset);

                return m_resetCommand;
            }
        }
        private void reset()
        {



            if (HasRecord)
            {

                revertInstance();
            }
            else
            {
                CurrentEmployee = new Employee();
                WorkSchedule = null;
            }
            referesh();
            //Messenger.Default.Send<Messages, ContactInformationViewModel>(Messages.Reset);
            Messenger.Default.Send<Messages, EmployeeWorkSchedule>(Messages.Reset);
            Messenger.Default.Register<WorkSchedule>(this, (workschedule) => this.WorkSchedule = workschedule);
            //RaisePropertyChanged(() => WorkSchedule);
            //Messenger.Default.Send<Messages, ContactInformationViewModel>(Messages.Reset);
            //Messenger.Default.Send<Messages, WorkScheduleViewModel>(Messages.Reset);
        }

        private void referesh()
        {
            clearErrors();
            beginUpdatingTarget();
            RaisePropertyChanged(() => this.FirstName);
            RaisePropertyChanged(() => this.LastName);
            RaisePropertyChanged(() => this.Gender);
            RaisePropertyChanged(() => this.NationalID);
            RaisePropertyChanged(() => this.IdentityNumber);
            RaisePropertyChanged(() => this.DateOfBirth);
            RaisePropertyChanged(() => this.ProfilePicture);
            RaisePropertyChanged(() => ContactInformations);
            RaisePropertyChanged(() => ContactInformationVideModels);
            RaisePropertyChanged(() => WorkSchedule);
            endUpdatingTarget();
        }


        private ICommand m_goBackCommand;
        /// <summary>
        /// بازگشت
        /// </summary>
        public ICommand GoBackCommand
        {
            get
            {
                if (m_goBackCommand == null)
                    m_goBackCommand = new RelayCommand(() =>
                    {
                        base.
                        clearErrors();
                        CurrentEmployee = null;
                        EditMode = false;
                        Messenger.Default.Send<Messages, MainViewModel>(Messages.HideEmployeeView);
                    });
                return m_goBackCommand;
            }
        }

        private bool m_isDeleted;
        /// <summary>
        /// حذف شده است؟
        /// </summary>
        public bool IsDeleted
        {
            set
            {
                m_isDeleted = value;
                RaisePropertyChanged(() => IsDeleted);
            }
            get
            {
                return m_isDeleted;
            }
        }
        private ICommand m_deleteCommand;

        /// <summary>
        /// حذف
        /// </summary>
        public ICommand DeleteCommand
        {
            get
            {
                if (m_deleteCommand == null)
                    m_deleteCommand = new RelayCommand(delete, () => HasRecord);

                return m_deleteCommand;
            }
        }
        private void delete()
        {
            MessageBoxResult result = MessageBoxResult.None;
            localizeMessageBox(() =>
            {
                result = MessageBox.Show(string.Format(fa_IR.DeletingEmployeeConfirmation, CurrentEmployee.FullName, CurrentEmployee.EmployeeID),
                    string.Format(fa_IR.DeletingEmployee, CurrentEmployee.FullName),
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning,
                    MessageBoxResult.No,
                    MessageBoxOptions.RightAlign
                );
            });

            if (result == MessageBoxResult.Yes)
            {

                string fullName = CurrentEmployee.FirstName + " " + CurrentEmployee.LastName ?? string.Empty;
                string employeeId = CurrentEmployee.EmployeeID.ToString() ?? string.Empty;

                FaceRecognitionService.DeleteFace(CurrentEmployee.ID.ToString());
                AutomatedAttendanceSystem.DeleteEmployee(CurrentEmployee);
                Messenger.Default.Send<Notification, MainViewModel>(new Notification() { Message = string.Format(fa_IR.EmployeeHasBeenDeletedSuccessfully, fullName, employeeId), Type = NotificationType.Info });
                Messenger.Default.Send<Messages, MainViewModel>(Messages.Update);
                RaisePropertyChanged(() => HasRecord);

                Messenger.Default.Send<Messages, MainViewModel>(Messages.HideEmployeeView);
            }
            else
            {
                Messenger.Default.Send<Messages, MainViewModel>(Messages.HideEmployeeView);
            }

        }

        private ICommand m_enableEditModeCommand;
        /// <summary>
        /// فعال کردن حالت ویرایش
        /// </summary>
        public ICommand EnableEditModeCommand
        {
            get
            {
                if (m_enableEditModeCommand == null)
                    m_enableEditModeCommand = new RelayCommand(toggleEditMode, () => HasRecord);
                return m_enableEditModeCommand;
            }
        }

        private ICommand m_showEmployeeCardCommand;
        /// <summary>
        /// نمایش کارت
        /// </summary>
        public ICommand ShowEmployeeCardCommand
        {
            get
            {
                if (m_showEmployeeCardCommand == null)
                {
                    m_showEmployeeCardCommand = new RelayCommand(showEmployeeCard, () => HasRecord);
                }
                return m_showEmployeeCardCommand;
            }
        }
        private void showEmployeeCard()
        {

            DocumentViewer documentViewer = new DocumentViewer();
            documentViewer.DataContext = this;
            documentViewer.LoadPage(new EmployeeCard());

            Content = documentViewer;
            //EmployeeCardViewer employeeCardViewer = new EmployeeCardViewer();
            //employeeCardViewer.ShowDialog();
        }

        private ICommand m_addContactInformationCommand;
        /// <summary>
        /// اضافه کردن اطلاعات تماس
        /// </summary>
        public ICommand AddContactInformationCommand
        {
            get
            {
                if (m_addContactInformationCommand == null)
                {
                    m_addContactInformationCommand = new RelayCommand(addContactInformation);
                }
                return m_addContactInformationCommand;
            }
        }
        private void addContactInformation()
        {

            CurrentEmployee.ContactInformations.Add(new ContactInformation() { Employee = CurrentEmployee });
            Messenger.Default.Send<Messages, ContactInformationViewModel>(Messages.EnableEditMode);

            RaisePropertyChanged(() => ContactInformationVideModels);
        }

        private ICommand m_removeContactInformationCommand;
        /// <summary>
        /// حذف اطلاعات تماس
        /// </summary>
        public ICommand RemoveContactInformationCommand
        {
            get
            {
                if (m_removeContactInformationCommand == null)
                {
                    m_removeContactInformationCommand = new RelayCommand<ContactInformationViewModel>(removeContactInformation, (o) => ContactInformations.Count > 0);
                }
                return m_removeContactInformationCommand;
            }
        }
        private void removeContactInformation(ContactInformationViewModel contactInformation)
        {
            if (CurrentEmployee.ContactInformations.Count > 0 && contactInformation != null)
            {

                CurrentEmployee.ContactInformations.Remove(contactInformation.ContactInformation);
                RaisePropertyChanged(() => ContactInformationVideModels);
            }
        }



        private ICommand m_ShowFRSManagementClientCommand;
        /// <summary>
        /// نمایش صفحه انتخاب عکس برای سیستم تشخیص چهره
        /// </summary>
        public ICommand ShowFRSManagementClientCommand
        {
            get
            {
                if (m_ShowFRSManagementClientCommand == null)
                {
                    m_ShowFRSManagementClientCommand = new RelayCommand<bool>(showFRSManagementClient, (o) => HasRecord);
                }
                return m_ShowFRSManagementClientCommand;
            }
        }
        private void showFRSManagementClient(bool open)
        {
            //Settings.Default.FRSManagementClientPath+"\\"+Settings.Default.FRSManagementClientExe,
            //.Exists()
            string fileName = string.Empty;
            if (!File.Exists(fileName = Settings.Default.FRSManagementClientExe) &&
                !File.Exists(fileName = Settings.Default.FRSManagementClientPath + Settings.Default.FRSManagementClientExe)
                )
            {


                Messenger.Default.Send<Notification, MainViewModel>(new Notification()
                {
                    Message = fa_IR.FRSManagementClientWasNotFound,
                    Type = NotificationType.Error
                });

                return;
            }

            string arguments = string.Format("key={0}", CurrentEmployee.ID);

            if (open)
                m_FRSManagementClient = Process.Start(fileName: fileName, arguments: arguments);
            else if (m_FRSManagementClient != null && m_FRSManagementClient.HasExited == false)
                m_FRSManagementClient.CloseMainWindow();

        }

        #endregion

        #region Constraints
        private bool HasRequiredField
        {
            get
            {

                return FirstName != null &&
                       LastName != null &&
                       DateOfBirth != DateTime.MinValue &&
                       (ProfilePicture != null && ProfilePicture != new Bitmap(1, 1)) &&
                       IdentityNumber != null &&
                       (NationalID != null && NationalID != IRNationalID.Empty);
            }
        }
        /// <summary>
        /// درون DataBase ثبت شده است
        /// </summary>
        public bool HasRecord
        {
            get
            {
                return CurrentEmployee != null && (CurrentEmployee.ID != null && CurrentEmployee.ID != Guid.Empty);
            }
        }
        #endregion
    }

}
