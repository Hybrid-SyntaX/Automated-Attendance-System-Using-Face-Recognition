using AAS.Entities;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using AAS.GUI.ExtensionMethods;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using AAS.ManagementClient.View.Reports;
using System.Collections.ObjectModel;
namespace AAS.ManagementClient.ViewModel
{
    /// <summary>
    /// EmployeeAttendanceTimesViewModel. مربوط به ساعات حضور یک کارمند
    /// </summary>
    public class EmployeeAttendanceTimesViewModel : AASViewModelBase
    {
        /// <summary>
        /// فیلتر وضیعت حضور
        /// </summary>
        public AttendanceStatus CurrentAttendanceStatusFitler { set; get; }


        /// <summary>
        /// فیلتر تاریخ شروع
        /// </summary>
        public DateTime StartDateFilter { set; get; }

        /// <summary>
        /// فیلتر تاریخ پایان
        /// </summary>
        public DateTime EndDateFilter { set; get; }

        /// <summary>
        /// استفاده از فیلتر تاریخ شروع
        /// </summary>
        public bool UseStartDateFilter { set; get; }

        /// <summary>
        /// استفاده از فیلتر تاریخ پایان
        /// </summary>
        public bool UseEndDateFilter { set; get; }

        private bool m_showAbsence;
        /// <summary>
        /// نمایش روز های غیبت
        /// </summary>
        public bool ShowAbsence
        {
            set
            {
                m_showAbsence = value;
                RaisePropertyChanged(() => ShowAbsence);
            }
            get
            {
                return m_showAbsence;
            }

        }

        private Employee m_employee;
        /// <summary>
        /// کارمند
        /// </summary>
        public Employee Employee
        {
            set
            {
                m_employee = value;
                RaisePropertyChanged(() => Employee);
            }
            get
            {
                return m_employee;
            }
        }

        private IList<DateTime> m_absenceTimes;
        private ICollection<DateTime> m_filteredAbsenceTimes;
        
         /// <summary>
         /// لیست تمام روز های غیبت
         /// </summary>
        public ICollection<DateTime> FilteredAbsenceTimes
        {
            set
            {
                m_filteredAbsenceTimes = value;
                RaisePropertyChanged(() => FilteredAbsenceTimes);
            }
            get
            {
                return m_filteredAbsenceTimes;
            }
        }

        private ICollection<AttendanceTime> m_filteredAttendanceTiles;

        /// <summary>
        /// لیست تمامی ساعات ورود فیلتر شده
        /// </summary>
        public ICollection<AttendanceTime> FilteredAttendanceTimes
        {
            set
            {
                m_filteredAttendanceTiles = value;
                RaisePropertyChanged(() => FilteredAttendanceTimes);
            }
            get
            {
                return m_filteredAttendanceTiles;
            }
        }

        private void retrieveAttendanceTimes()
        {
            if (Employee != null && Employee.AttendanceTimes != null)
            {
                FilteredAttendanceTimes = Employee.AttendanceTimes;
                retrieveAbsenceTimes();
                //  filterEmployeeAttendanceTimes();
            }
        }
        private void retrieveAbsenceTimes()
        {
            m_absenceTimes = new List<DateTime>();
            if (Employee != null && Employee.AttendanceTimes != null)
            {
                for (DateTime date = m_employee.DateOfEmployement.Date; date <= DateTime.Now.Date; date = date.AddDays(1))
                {
                    int presentCount = Employee.AttendanceTimes.Count((at) => at.EntryTime.Date == date || at.ExitTime == date);
                    if (presentCount == 0)
                        m_absenceTimes.Add(date);
                }

            }
            FilteredAbsenceTimes = new ObservableCollection<DateTime>(m_absenceTimes);
        }

        /// <summary>
        /// متد سازنده
        /// </summary>
        public EmployeeAttendanceTimesViewModel()
        {
            Messenger.Default.Register<Employee>(this, (employee) =>
            {
                Employee = employee;
                retrieveAttendanceTimes();
            });
            EndDateFilter = DateTime.Now;
            StartDateFilter = EndDateFilter.AddMonths(-1);

            retrieveAttendanceTimes();
            retrieveAbsenceTimes();
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

            if (ShowAbsence)
                documentViewer.LoadPage(new EmployeeAbsenceTimes());
            else
                documentViewer.LoadPage(new EmployeeAttendanceTimesReport());
            documentViewer.Print();
        }

  
        private ICommand m_filterEmployeeAttendanceTimesCommand;
        /// <summary>
        /// اعمال فیلتر
        /// </summary>
        public ICommand FilterEmployeeAttendanceTimesCommand
        {
            get
            {
                if (m_filterEmployeeAttendanceTimesCommand == null)
                {
                    m_filterEmployeeAttendanceTimesCommand = new RelayCommand(filterEmployeeAttendanceTimes);
                }
                return m_filterEmployeeAttendanceTimesCommand;
            }
        }



        private void filterEmployeeAttendanceTimes()
        {
            Func<DateTime, bool> onStartDate = (absenceTime) =>
            {
                return UseStartDateFilter ? absenceTime.Date >= StartDateFilter : true;
            };
            Func<DateTime, bool> onEndDate = (absenceTime) =>
            {
                return UseEndDateFilter ?
                                      absenceTime.Date <= EndDateFilter : true;
            };

            if (CurrentAttendanceStatusFitler == AttendanceStatus.Absent)
            {

                if (FilteredAbsenceTimes.Count == 0)
                    retrieveAbsenceTimes();
                ShowAbsence = true;

                if (StartDateFilter != DateTime.MinValue || EndDateFilter != DateTime.MinValue)
                {

                    IEnumerable<DateTime> l_absenceTimesInRange = null;


                    l_absenceTimesInRange = from absenceTime in m_absenceTimes
                                            where
                                            onStartDate(absenceTime)
                                                &&
                                            onEndDate(absenceTime)
                                            select absenceTime;

                    FilteredAbsenceTimes = new ObservableCollection<DateTime>(l_absenceTimesInRange);
                }

                return;
            }
            else if (CurrentAttendanceStatusFitler == AttendanceStatus.Present)
            {
                ShowAbsence = false;


                IEnumerable<AttendanceTime> l_present = null;
                if (Employee != null && Employee.AttendanceTimes != null)
                {

                    l_present = from at in Employee.AttendanceTimes
                                where
                                (onStartDate(at.EntryTime.Date) || onStartDate(at.ExitTime.Date))
                                &&
                                (onEndDate(at.EntryTime.Date) || onEndDate(at.ExitTime))

                                select at;


                    FilteredAttendanceTimes = new ObservableCollection<AttendanceTime>(l_present);
                }

            }
        }

    }
}
