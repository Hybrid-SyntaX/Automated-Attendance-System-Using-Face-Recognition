using AAS.Proxy;
using AAS.Entities;
using Emgu.CV;
using GalaSoft.MvvmLight;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Collections.Generic;
using Emgu.CV.Structure;
using System.Drawing;
using AM.FRS.Util;
using AM.FRS.Contracts;
using AM.QRService.Contracts;
using AAS.Service;
using System.Timers;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Diagnostics;
using AAS.Client.AttendanceMethods;
namespace AAS.Client.GUI.ViewModel
{
    /// <summary>
    /// MainViewModel
    /// </summary>
    public class MainViewModel : ViewModelBase
    {

        #region Services
        private IAutomatedAttendanceSystem AutomatedAttendanceSystem;
        private IQRService QrService;
        private IFaceRecognitionService FaceRecognition;
        #endregion

        #region Fields
        private Employee m_lastEmployeeCache;

        private Timer m_restTimer;
        #endregion

        #region Properties
        private Bitmap m_captureImage;

        /// <summary>
        /// فریم جاری
        /// </summary>
        public Bitmap CaptureImage
        {
            private set
            {
                m_captureImage = value;
                RaisePropertyChanged(() => CaptureImage);
            }
            get
            {
                return m_captureImage;
            }
        }


        private Employee m_detectedEmployee;
        /// <summary>
        /// کارمند تشخیص داده شده
        /// </summary>
        public Employee DetectedEmployee
        {
            set
            {
                m_detectedEmployee = value;
                RaisePropertyChanged(() => DetectedEmployee);
                RaisePropertyChanged(() => GenderTitle);
                RaisePropertyChanged(() => IsEmployeeDetected);
            }
            get
            {
                return m_detectedEmployee;
            }
        }

        private EmployeeID m_enteredEmployeeID;
        /// <summary>
        /// کد کارمندی وارد شده توسط کارمند
        /// </summary>
        public EmployeeID EnteredEmployeeID
        {
            set
            {
                m_enteredEmployeeID = value;
                RaisePropertyChanged(() => EnteredEmployeeID);
            }
            get
            {
                return m_enteredEmployeeID;
            }
        }

        /// <summary>
        /// کارمندی تشخیص داده شده؟
        /// </summary>
        public bool IsEmployeeDetected
        {
            get
            {

                return DetectedEmployee != null;
            }
        }

        /// <summary>
        /// فعال سازی ثبت گزارش خطا
        /// </summary>
        private bool EnableLogging = true;

        private bool m_rejected;

        /// <summary>
        /// کارمند شناسایی شده رد شده؟
        /// </summary>
        public bool Rejected
        {
            set
            {
                m_rejected = value;
                RaisePropertyChanged(() => Rejected);
            }
            get
            {
                return m_rejected;
            }
        }
        #endregion

        #region Messages

        private string m_entryOrExit;

        /// <summary>
        /// نمایش رشته "خروج" یا "ورود"
        /// </summary>
        public string EntryOrExit
        {
            set
            {
                m_entryOrExit = value;
                RaisePropertyChanged(() => EntryOrExit);
            }
            get
            {
                return m_entryOrExit;
            }
        }

        private DateTime m_entryOrExitHour;
        /// <summary>
        /// نمایش ساعت ورود یا خروج بر روی صفحه
        /// </summary>
        public DateTime EntryOrExitHour
        {
            set
            {
                m_entryOrExitHour = value;
                RaisePropertyChanged(() => EntryOrExitHour);
            }
            get
            {
                return m_entryOrExitHour;
            }
        }

        /// <summary>
        /// رشته "آقا" یا "خانم"
        /// </summary>
        public string GenderTitle
        {
            get
            {
                if (DetectedEmployee != null)
                    if (DetectedEmployee.Gender == Gender.Male)
                        return (string)Application.Current.FindResource("Mr");
                    else
                        return (string)Application.Current.FindResource("Ms");

                return string.Empty;
            }
        }

        private string m_successMessage;
        /// <summary>
        /// پیغام مربوط به موفقیت آمیز بودن
        /// </summary>
        public string SuccessMessage
        {
            set
            {
                m_successMessage = value;
                RaisePropertyChanged(() => SuccessMessage);
            }
            get
            {

                return m_successMessage;
            }
        }

        private string m_errorMessage;
        /// <summary>
        /// پیغام خطا
        /// </summary>
        public string ErrorMessage
        {
            set
            {
                m_errorMessage = value;
                RaisePropertyChanged(() => ErrorMessage);
            }
            get
            {
                return m_errorMessage;
            }
        }

        private string m_alternativeMethodMessage;
        /// <summary>
        /// پیغام پیشنهاد استفاده از روش اعلام حضور دیگر
        /// </summary>
        public string AlternativeMethodMessage
        {
            set
            {
                m_alternativeMethodMessage = value;
                RaisePropertyChanged(() => AlternativeMethodMessage);
            }
            get
            {
                return m_alternativeMethodMessage;
            }
        }

        /// <summary>
        /// نمایش پیغام شکست
        /// </summary>
        /// <param name="name"></param>
        private void showFailedMessage(string name)
        {
            ErrorMessage = (string)Application.Current.FindResource(name);
            AlternativeMethodMessage = (string)Application.Current.TryFindResource(name + "AlternativeMethod") ?? string.Empty;
        }
        #endregion

        private void log(Log log)
        {
            if (log.EventTime == DateTime.MinValue)
                log.EventTime = DateTime.Now;
            m_logs.Add(log);
        }
        #region Attendance Methods
        private AttendanceContext faceRecognitionMethod;
        private AttendanceContext QRMethod;
        private AttendanceContext ManualMethod;
        private void intitalizeAttendanceMethods()
        {
            faceRecognitionMethod = new AttendanceContext(new FaceRecognitionMethod());
            faceRecognitionMethod.Success += faceRecognitionMethod_Success;
            faceRecognitionMethod.Failure += faceRecognitionMethod_Failure;
            faceRecognitionMethod.CheckSuccess += faceRecognitionMethod_CheckSuccess;
            faceRecognitionMethod.CheckFailure += faceRecognitionMethod_CheckFailure;
            QRMethod = new AttendanceContext(new QRMethod());
            QRMethod.Success += QRMethod_Success;
            QRMethod.Failure += QRMethod_Failure;
            QRMethod.CheckFailure += QRMethod_CheckFailure;
            ManualMethod = new AttendanceContext(new ManualMethod());
            ManualMethod.Success += ManualMethod_Success;
            ManualMethod.Failure += ManualMethod_Failure;

            attendanceMethods = new List<AttendanceContext>() { faceRecognitionMethod, QRMethod, ManualMethod };
        }

        private List<AttendanceContext> attendanceMethods;
        private List<Log> m_logs;
        private void faceRecognitionMethod_Success(object sender, AttendanceMethodEventArgs e)
        {
            //(sender as FaceRecognitionMethod).DrawFacesBoundaries(EmguCVHelper.CaptureImage);

        }
        private void faceRecognitionMethod_Failure(object sender, AttendanceMethodEventArgs e)
        {
            showFailedMessage("FRSFailed");
            if (EnableLogging)
                log(new Log()
                {
                    AttendanceMethod = AttendanceMethod.FaceRecognition,
                    AttendanceMethodResult = AttendanceMethodResult.Failure,
                    Picture = ((Image<Bgr, byte>)e.Input).ToBitmap()
                });
            //AutomatedAttendanceSystem.CreateLog(AttendanceMethod.FaceRecognition,
            //AttendanceMethodResult.Failure, ((Image<Bgr, byte>)e.Input).ToBitmap());
        }
        private void faceRecognitionMethod_CheckSuccess(object sender, AttendanceMethodEventArgs e)
        {
            Image<Bgr,byte> img = EmguCVHelper.CaptureImage;
            (sender as FaceRecognitionMethod).DrawFacesBoundaries(img);
            CaptureImage = img.ToBitmap();
        }
        private void faceRecognitionMethod_CheckFailure(object sender, AttendanceMethodEventArgs e)
        {
            showFailedMessage("FRSFailed");
        }
        private void QRMethod_Failure(object sender, AttendanceMethodEventArgs e)
        {
            showFailedMessage("QRFailed");

            if (EnableLogging)
                log(new Log()
                {
                    AttendanceMethod = AttendanceMethod.QRCode,
                    AttendanceMethodResult = AttendanceMethodResult.Failure,
                    Picture = (((Image<Bgr, byte>)e.Input).ToBitmap())
                });
        }
        private void QRMethod_CheckFailure(object sender, AttendanceMethodEventArgs e)
        {

        }
        private void QRMethod_Success(object sender, AttendanceMethodEventArgs e)
        {
            if (EnableLogging)
                log(new Log()
                {
                    AttendanceMethod = AttendanceMethod.QRCode,
                    AttendanceMethodResult = AttendanceMethodResult.Success,
                    Picture = ((Image<Bgr, byte>)e.Input).ToBitmap(),
                    Employee = this.DetectedEmployee
                });
        }
        private void ManualMethod_Failure(object sender, AttendanceMethodEventArgs e)
        {
            showFailedMessage("ManualFailed");
            if (EnableLogging)
                log(new Log()
                {
                    AttendanceMethod = AttendanceMethod.ManualEmployeeIDEntry,
                    AttendanceMethodResult = AttendanceMethodResult.Failure,
                    Picture = ((Image<Bgr, byte>)e.Input).ToBitmap(),
                    EnteredEmployeeID = EnteredEmployeeID.ToString()
                });
        }
        private void ManualMethod_Success(object sender, AttendanceMethodEventArgs e)
        {
            if (EnableLogging)
                log(new Log()
                {
                    AttendanceMethod = AttendanceMethod.ManualEmployeeIDEntry,
                    AttendanceMethodResult = AttendanceMethodResult.Success,
                    Picture = EmguCVHelper.CaptureImage.ToBitmap(),
                    Employee = this.DetectedEmployee,
                    EnteredEmployeeID = EnteredEmployeeID.ToString()
                });
        }
        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// متد سازنده
        /// </summary>
        public MainViewModel()
        {
            if (!IsInDesignMode)
            {

                try
                {
                    //Proxies are created
                    QrService = new QRServiceProxy();
                    AutomatedAttendanceSystem = new AAS.Proxy.AutomatedAttendanceSystemProxy();
                    FaceRecognition = new FaceRecognitionServiceProxy();

                    intitalizeAttendanceMethods();
                    EmguCVHelper.StartCapture();
                    EmguCVHelper.CaptureImageIsRefreshed += RefreshImage;
                    m_restTimer = new Timer(3000);

                    m_restTimer.Elapsed += restTimer_Elapsed;
                }
                catch (Exception ex)
                {
                    Exception exep = ex;
                    {
                        MessageBox.Show(exep.Message);
                    } while ((exep = exep.InnerException) != null) ;
                }
                m_logs = new List<Log>();
            }
            else
            {
                Employee employee = new Employee()
                {
                    FirstName = "احمد",
                    LastName = "رحیمی",
                    Gender = Gender.Male,
                    DateOfBirth = new DateTime(1980, 01, 01),
                    DateOfEmployement = new DateTime(2000, 04, 12),
                    IdentityNumber = "13654",
                    NationalID = new IRNationalID("000-000004-3"),
                    EmployeeID = new EmployeeID(0, 4, 1),
                    ProfilePicture = new Bitmap(@"G:\_DATA\dummy.png"),
                };
                DetectedEmployee = employee;
                //DetectedEmployee = null;
                ErrorMessage = (string)Application.Current.FindResource("FRSCouldNotDetect");
                AlternativeMethodMessage = (string)Application.Current.FindResource("FRSCouldNotDetectAlternativeMethod");
                EntryOrExit = string.Format((string)Application.Current.FindResource("Entry"));
                EntryOrExitHour = DateTime.Now;

            }

        }
        /// <summary>
        /// متد خرابنده
        /// </summary>
        ~MainViewModel()
        {
            EmguCVHelper.StopCapture();
        }
        #endregion

        #region Vision

        private void RefreshImage(object sender, EventArgs e)
        {
            ///[Refresh image]
            if (recognizeEmployee())
            {
                registerAttendance();
            }
            RaisePropertyChanged(() => IsEmployeeDetected);
            CaptureImage = EmguCVHelper.CaptureImage.ToBitmap();
            ///[Refresh image]
        }




        private bool recognizeEmployee()
        {
            ///[Recognizing employee using various methods]
            Guid l_id = faceRecognitionMethod.UpdateInput(EmguCVHelper.CaptureImage).Run();

            if (l_id == Guid.Empty)
                l_id = QRMethod.UpdateInput(EmguCVHelper.CaptureImage).Run();

            if (l_id == Guid.Empty)
                l_id = ManualMethod.UpdateInput(EnteredEmployeeID).Run();


            if (l_id != Guid.Empty)
            {
                if (DetectedEmployee == null)
                    DetectedEmployee = retrieveEmployee(l_id);
            }

            if ((EnteredEmployeeID != null) &&
                (l_id == Guid.Empty || DetectedEmployee == null))
            {


                DetectedEmployee = null;
                return false;
            }
            else return true;
            ///[Recognizing employee using various methods]
        }


        AttendanceTime m_attendanceTime;
        void restTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ///[Timer elapsed]
            m_attendanceTime = null;

            EmguCVHelper.CaptureImageIsRefreshed += RefreshImage;
            m_restTimer.Stop();
            if (Rejected)
            {

                DetectedEmployee = null;
                EnteredEmployeeID = null;
                RaisePropertyChanged(() => IsEmployeeDetected);

            }
            else
            {
                m_attendanceTime = AutomatedAttendanceSystem.RegisterAttendance(DetectedEmployee);

                showSubmitAttendanceMessage();
                RaisePropertyChanged(() => IsRegistered);
            }
            Rejected = false;
            DetectedEmployee = null;
            EnteredEmployeeID = null;
            saveLogs();
            RaisePropertyChanged(() => IsEmployeeDetected);
            ///[Timer elapsed]
        }
        void saveLogs()
        {

            AutomatedAttendanceSystem.CreateLogs(m_logs);

            m_logs.Clear();
        }

        /// <summary>
        /// آیا زمان ورود ثبت شده است؟
        /// </summary>
        public bool IsRegistered
        {
            get
            {
                return m_attendanceTime != null;
            }
        }
        #endregion

        #region Database
        private Employee retrieveEmployee(Guid l_id)
        {
            if (m_lastEmployeeCache != null && m_lastEmployeeCache.ID.Equals(l_id))
                return m_lastEmployeeCache;
            else return m_lastEmployeeCache = AutomatedAttendanceSystem.RetrieveEmployee(l_id);
        }

        private void registerAttendance()
        {
            if (DetectedEmployee != null)
            {
                //reset everything
                EmguCVHelper.CaptureImageIsRefreshed -= RefreshImage;
                m_restTimer.Start();
                //do attendance here
                //AttendanceTime attendanceTime = AutomatedAttendanceSystem.RegisterAttendance(DetectedEmployee);
                //AttendanceTime attendanceTime = new AttendanceTime()
                //{
                //    Employee = DetectedEmployee,
                //    EntryTime = DateTime.Now.AddHours(-9),
                //    ExitTime = DateTime.Now
                //};


            }
        }

        private void showSubmitAttendanceMessage()
        {
            if (m_attendanceTime != null)
            {

                if (m_attendanceTime.ExitTime == null || m_attendanceTime.ExitTime == DateTime.MinValue)
                {
                    EntryOrExit = string.Format((string)Application.Current.FindResource("Entry"));
                    EntryOrExitHour = m_attendanceTime.EntryTime;
                }
                else
                {
                    EntryOrExit = string.Format((string)Application.Current.FindResource("Exit"));
                    EntryOrExitHour = m_attendanceTime.ExitTime;
                }
            }
        }
        #endregion

        #region Commands

        private ICommand m_useManualAttendanceMethodCommand;

        /// <summary>
        /// استفاده از روش درج دستی کد کارمندی
        /// </summary>
        public ICommand UseManualAttendanceMethodCommand
        {
            get
            {
                if (m_useManualAttendanceMethodCommand == null)
                {
                    m_useManualAttendanceMethodCommand = new RelayCommand<string>(useManualAttendanceMethod);
                }
                return m_useManualAttendanceMethodCommand;
            }
        }
        private void useManualAttendanceMethod(string input)
        {
            EmployeeID employeeId = EmployeeID.Empty;
            if (EmployeeID.TryParse(input, out employeeId))
                EnteredEmployeeID = employeeId;
        }

        private ICommand m_rejectCommand;

        /// <summary>
        /// رد کردن تشخیص
        /// </summary>
        public ICommand RejectCommand
        {
            get
            {
                if (m_rejectCommand == null)
                {
                    m_rejectCommand = new RelayCommand(reject);
                }
                return m_rejectCommand;
            }
        }
        private void reject()
        {
            DetectedEmployee = null;
            EnteredEmployeeID = null;

            RaisePropertyChanged(() => IsEmployeeDetected);
            EmguCVHelper.CaptureImageIsRefreshed += RefreshImage;
            m_restTimer.Stop();
        }

        #endregion
    }
}