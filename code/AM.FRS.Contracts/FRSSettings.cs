using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AM.FRS.Contracts
{
    [DataContract]
    /// تنظیمات سیتسم تشخیص چهره
    public class FRSSettings : INotifyPropertyChanged
    {
        private double m_EigenFaceRecognizerThreshold;
        [DataMember]
        ///حد آستانه طول
        public double EigenFaceRecognizerThreshold
        {
            set
            {
                m_EigenFaceRecognizerThreshold = value;
                NotifyPropertyChanged("EigenFaceRecognizerThreshold");
            }
            get
            {
                return m_EigenFaceRecognizerThreshold;
            }
        }

        private bool m_EigenFaceRecognizerThresholdInfinity;
        [DataMember]
        ///حد آستانه طول بی نهایت 
        public bool EigenFaceRecognizerThresholdInfinity
        {
            set
            {
                m_EigenFaceRecognizerThresholdInfinity = value;
                NotifyPropertyChanged("EigenFaceRecognizerThresholdInfinity");
            }
            get
            {
                return m_EigenFaceRecognizerThresholdInfinity;
            }
        }

        private Size m_CascadeClassifierMaxSize;
        [DataMember]
        ///<summary>
        ///بزرگترین اندازه پنجره
        ///<summary>
        public Size CascadeClassifierMaxSize
        {
            set
            {
                m_CascadeClassifierMaxSize = value;
                NotifyPropertyChanged("CascadeClassifierMaxSize");
            }
            get
            {
                return m_CascadeClassifierMaxSize;

            }
        }

        /// <summary>
        /// مقدار طول بزرگترین اندازه پنجره
        /// </summary>
        public int CascadeClassifierMaxSizeHeight
        {
            set
            {
                m_CascadeClassifierMaxSize.Height = value;
                NotifyPropertyChanged("CascadeClassifierMaxSizeHeight");
                NotifyPropertyChanged("CascadeClassifierMaxSize");
            }
            get
            {
                return m_CascadeClassifierMaxSize.Height;

            }
        }

        /// <summary>
        /// مقدار عرض بزرگترین اندازه پنجره
        /// </summary>
        public int CascadeClassifierMaxSizeWidth
        {
            set
            {
                m_CascadeClassifierMaxSize.Width = value;
                NotifyPropertyChanged("CascadeClassifierMaxSizeWidth");
                NotifyPropertyChanged("CascadeClassifierMaxSize");
            }
            get
            {
                return m_CascadeClassifierMaxSize.Width;

            }
        }

        private Size m_CascadeClassifierMinSize;
        [DataMember]
        ///<summary>
        ///کمترین اندازه پنجره
        ///<summary>
        public Size CascadeClassifierMinSize
        {
            set
            {
                m_CascadeClassifierMinSize = value;
                NotifyPropertyChanged("CascadeClassifierMinSize");
            }
            get
            {
                return m_CascadeClassifierMinSize;
            }
        }


        /// <summary>
        /// مقدار طول کوچکترین اندازه پنجره
        /// </summary>
        public int CascadeClassifierMinSizeHeight
        {
            set
            {
                m_CascadeClassifierMinSize.Height = value;
                NotifyPropertyChanged("CascadeClassifierMinSizeHeight");
                NotifyPropertyChanged("CascadeClassifierMinSize");
            }
            get
            {
                return m_CascadeClassifierMinSize.Height;

            }
        }

        /// <summary>
        /// مقدار عرض کوچکترین اندازه پنجره
        /// </summary>
        public int CascadeClassifierMinSizeWidth
        {
            set
            {
                m_CascadeClassifierMinSize.Width = value;
                NotifyPropertyChanged("CascadeClassifierMinSizeWidth");
                NotifyPropertyChanged("CascadeClassifierMinSize");
            }
            get
            {
                return m_CascadeClassifierMinSize.Width;

            }
        }

        private int m_CascadeClassifierMinNeighbours;
        [DataMember]
        ///<summary>
        ///کمترین تعداد  مستطیل همسایه ها (منهای 1) که یک شی را می سازند.
        ///گروه های کوچک تر از این مقدار چشم پوشی می شوند
        ///اگر مقدار 0 باشد، هیچ گروه بندی انجام نمی شود و تمام موارد تشخیص داده شده بر گردانده می شود
        ///<summary>
        public int CascadeClassifierMinNeighbours
        {
            set
            {
                m_CascadeClassifierMinNeighbours = value;
                NotifyPropertyChanged("CascadeClassifierMinNeighbours");
            }
            get
            {
                return m_CascadeClassifierMinNeighbours;
            }

        }

        private double m_CascadeClassifierScaleFactor;
        [DataMember]
        ///<summary>
        ///فاکتور مقیاسی که در آن مقیاس پنجره جستجو در جستجوی های بعدی به این میزان تغییر می کند 
        ///<summary>
        public double CascadeClassifierScaleFactor
        {
            set
            {
                m_CascadeClassifierScaleFactor = value;
                NotifyPropertyChanged("CascadeClassifierScaleFactor");
            }
            get
            {
                return m_CascadeClassifierScaleFactor;
            }
        }

        
        private int m_EigenFaceRecognizerNumComponents;
        [DataMember]
        ///تعداد مولفه ها
        ///در صورت ورود 0 حالت پیشفرض استفاده می شود
        public int EigenFaceRecognizerNumComponents
        {
            set
            {
                m_EigenFaceRecognizerNumComponents = value;
                NotifyPropertyChanged("EigenFaceRecognizerNumComponents");
            }
            get
            {
                return m_EigenFaceRecognizerNumComponents;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// اطلاع رسانی هنگام تغییر Property
        /// </summary>
        /// <param name="propertyName">نام Property</param>
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
