using AAS.Entities;
using AAS.Proxy;
using AAS.Service;
using AM.FRS.Contracts;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Input;
using System.Globalization;
using System.Windows.Controls;
using AAS.LogViewer.Reports;
using AAS.GUI.ExtensionMethods;
using AM.FRS.Util;
using Emgu.CV;
using Emgu.CV.Structure;
using AM.QRService.Contracts;
using AAS.Proxy;
namespace AAS.LogViewer.ViewModel
{
    /// <summary>
    /// LogViewer MainViewModel
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Services
        private IAutomatedAttendanceSystem AutomatedAttendanceSystem;
        private IFaceRecognitionService FaceRecognitionService;
        private IQRService QRService;
        #endregion

        private Log m_selectedLogFilter;
        /// <summary>
        /// گزارش انتخاب شده
        /// </summary>
        public Log SelectedLogFilter
        {
            set
            {
                m_selectedLogFilter = value;
                RaisePropertyChanged(() => SelectedLogFilter);
             //   m_detectedFaces.Clear();

            }
            get
            {
                return m_selectedLogFilter;
            }
        }
        private FRSSettings m_frsSettings;
   
        /// <summary>
        /// تنظیمات سیستم تشخیص چهره
        /// </summary>
        public FRSSettings FRSSettings
        {
            set
            {
                m_frsSettings = value;
                RaisePropertyChanged(() => FRSSettings);
            }
            get
            {
                return m_frsSettings;
            }

        }

        #region MainViewModel Members
        private ICollection<Log> m_logs;

        private ICollection<Log> m_filteredLogs;

        /// <summary>
        /// مجموعه فیلتر شده تمام گزارش ها
        /// </summary>
        public ICollection<Log> FilteredLogs
        {
            set
            {
                m_filteredLogs = value;
                RaisePropertyChanged(() => FilteredLogs);

            }
            get
            {
                return m_filteredLogs;
            }
        }
        #endregion

        #region Constructor & Destructor
        /// <summary>
        /// متذ سازنده
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                //Logs = new ObservableCollection<Log>()
                //{
                //    new Log()
                //    {
                //        Employee=new Employee(){FirstName="Abbas",LastName="Allahyari"},
                //        EventTime=DateTime.Now,
                //        AttendanceMethod=AttendanceMethod.FaceDetection,
                //        AttendanceMethodResult=AttendanceMethodResult.Failure
                //    },
                //};
            }
            else
            {

                AutomatedAttendanceSystem = new AutomatedAttendanceSystemProxy();
                FaceRecognitionService = new FaceRecognitionServiceProxy();
                QRService = new QRServiceProxy();

                this.FRSSettings = FaceRecognitionService.GetSettings(false);
                retrieveLogs();
            }
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("fa-IR");
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("fa-IR");
        }

        private void retrieveLogs()
        {
            FilteredLogs = m_logs = new ObservableCollection<Log>(AutomatedAttendanceSystem.RetrieveLogs());
        }
        #endregion

        /// <summary>
        /// فیلتر مروبط به روش اعلام حضور
        /// </summary>
        public AttendanceMethod AttendanceMethodFilter { set; get; }
        
        /// <summary>
        /// فیلتر نتیجه اعلام حضور
        /// </summary>
        public AttendanceMethodResult AttendanceMethodResultFilter { set; get; }

        /// <summary>
        /// فیلتر تاریخ
        /// </summary>
        public DateTime DateFilter { set; get; }

        private bool m_useAttendanceMethodFilter;

        /// <summary>
        /// استفاده از فیلتر اعلام حضور
        /// </summary>
        public bool UseAttendanceMethodFilter
        {
            set
            {
                m_useAttendanceMethodFilter = value;
                RaisePropertyChanged(() => UseAttendanceMethodFilter);
            }
            get
            {
                return m_useAttendanceMethodFilter;
            }
        }

        private bool m_useAttendanceMethodResultFilter;

        /// <summary>
        /// استفاده از فیلتر نتیجه اعلام حضور
        /// </summary>
        public bool UseAttendanceMethodResultFilter
        {
            set
            {
                m_useAttendanceMethodResultFilter = value;
                RaisePropertyChanged(() => UseAttendanceMethodResultFilter);
            }
            get
            {
                return m_useAttendanceMethodResultFilter;
            }
        }

        private bool m_useDateFilter;
        
        /// <summary>
        /// استفاده از فیلتر تاریخ
        /// </summary>
        public bool UseDateFilter
        {
            set
            {
                m_useDateFilter = value;
                RaisePropertyChanged(() => UseDateFilter);
            }
            get
            {

                return m_useDateFilter;
            }
        }

        #region Commands
        private ICommand m_detectAndRecognizeFaceCommand;

        /// <summary>
        /// تشخیص و شناسایی چهره
        /// </summary>
        public ICommand DetectAndRecognizeFaceCommand
        {
            get
            {
                if (m_detectAndRecognizeFaceCommand == null)
                {
                    m_detectAndRecognizeFaceCommand = new RelayCommand(detectAndRecognizeFace);
                }
                return m_detectAndRecognizeFaceCommand;
            }
        }

        private Bitmap m_detectedFace;
        /// <summary>
        /// صورت تشخیص داده شده
        /// </summary>
        public Bitmap DetectedFace
        {
            set
            {

                m_detectedFace = value;
                RaisePropertyChanged(() => DetectedFace);
            }
            get
            {
                return m_detectedFace;
            }
        }

        private List<DetectedObject> m_detectedFaces;
      
        /// <summary>
        /// صورت های تشخیص داده شده
        /// </summary>
        public List<DetectedObject> DetectedFaces
        {
            set
            {
                m_detectedFaces = value;
                RaisePropertyChanged(() => DetectedFaces);
            }
            get
            {
                return m_detectedFaces;
            }
        }
        private string m_qrCodeContent;

        /// <summary>
        /// مقدار موجود در کد QR
        /// </summary>
        public string QRCodeContent
        {
            set
            {
                m_qrCodeContent = value;
                RaisePropertyChanged(() => QRCodeContent);
            }
            get
            {
                return m_qrCodeContent;
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
                RaisePropertyChanged(() => IsEmployeeDetected);
            }
            get
            {
                return m_detectedEmployee;
            }
        }

        /// <summary>
        /// کارمند تشخیص داده شده؟
        /// </summary>
        public bool IsEmployeeDetected { get { return DetectedEmployee != null; } }
        private void detectAndRecognizeFace()
        {
            //needs preproessing
            if (SelectedLogFilter != null)
                if (SelectedLogFilter.Picture != null)
                {

                    Image<Bgr, byte> image = new Image<Bgr, byte>(SelectedLogFilter.Picture);
                    DetectedFaces = FaceRecognitionService.DetectFace(EmguCVHelper.ScaleImage(image, 0.25).ToBitmap());
                    QRCodeContent = (string)QRService.Read(SelectedLogFilter.Picture);

                    Guid id = Guid.Empty;
                    int i = 0;
                    if (DetectedFaces.Count > 0)
                        do
                        {
                            string str = FaceRecognitionService.RecognizeFace(DetectedFaces[i].Bitmap);
                            Guid.TryParse(str, out id);
                            i++;
                        } while (id != Guid.Empty && i < DetectedFaces.Count);

                    if (id != Guid.Empty)
                        DetectedEmployee = AutomatedAttendanceSystem.RetrieveEmployee(id);
                    else DetectedEmployee = null;
                    //if (DetectedFaces.Count > 0)
                    //{
                    //    EmguCVHelper.DrawObjectsBoundaries(image, DetectedFaces, 0.25);

                    //    SelectedLogFilter.Picture = image.ToBitmap();

                    //}
                }
        }


        private ICommand m_filterLogsCommand;
        /// <summary>
        /// اعلام فیلتر
        /// </summary>
        public ICommand FilterLogsCommand
        {
            get
            {
                if (m_filterLogsCommand == null)
                {
                    m_filterLogsCommand = new RelayCommand(filterLogs);
                }
                return m_filterLogsCommand;
            }
        }
        private void filterLogs()
        {
            ///[Filtering logs]
            Func<Log, bool> withAttendanceMethodFilter = (log) =>
            {
                return UseAttendanceMethodFilter ? log.AttendanceMethod == AttendanceMethodFilter : true;
            };
            Func<Log, bool> withAttendanceMethodResultFilter = (log) =>
            {
                return UseAttendanceMethodResultFilter ? log.AttendanceMethodResult == AttendanceMethodResultFilter : true;
            };
            Func<Log, bool> withDateFilter = (log) =>
            {
                return UseDateFilter ? log.EventTime.Date == DateFilter.Date : true;
            };


            IEnumerable<Log> l_matchingLogs = from log in m_logs
                                              where
                                                    withDateFilter(log) &&
                                                    withAttendanceMethodFilter(log) &&
                                                    withAttendanceMethodResultFilter(log)
                                              select log;

            FilteredLogs = new ObservableCollection<Log>(l_matchingLogs);
            ///[Filtering logs]
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
                    m_printCommand = new RelayCommand(print);
                }
                return m_printCommand;
            }
        }
        private void print()
        {
            DocumentViewer documentViewer = new DocumentViewer();
            documentViewer.DataContext = this;

            documentViewer.LoadPage(new LogReport());
            documentViewer.Print();
        }

        private ICommand m_saveSettingsCommand;
        /// <summary>
        /// ذخیره تنظیات
        /// </summary>
        public ICommand SaveSettingsCommand
        {
            get
            {
                if (m_saveSettingsCommand == null)
                {
                    m_saveSettingsCommand = new RelayCommand(saveSettings);
                }
                return m_saveSettingsCommand;
            }
        }
        private void saveSettings()
        {
            FaceRecognitionService.SetSettings(FRSSettings);
            DetectAndRecognizeFaceCommand.Execute(null);
        }


        private ICommand m_restoreDefaultSettingsCommand;
        /// <summary>
        /// بارگذاری تنظیمات پیشفرض
        /// </summary>
        public ICommand RestoreDefaultSettingsCommand
        {
            get
            {
                if (m_restoreDefaultSettingsCommand == null)
                {
                    m_restoreDefaultSettingsCommand = new RelayCommand(restoreDefaultSettings);
                }
                return m_restoreDefaultSettingsCommand;
            }
        }
        private void restoreDefaultSettings()
        {
            FRSSettings = FaceRecognitionService.GetSettings(true);
        }

        #endregion
    }
}