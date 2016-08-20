/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:AAS.GUI"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace AAS.ManagementClient.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            
            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}
            //SimpleIoc.Default.Register<EmployeeViewModel>();
            SimpleIoc.Default.Register<EmployeeViewModel>(true);
            SimpleIoc.Default.Register<ContactInformationViewModel>(true);
           
            SimpleIoc.Default.Register<MainViewModel>(true);
            SimpleIoc.Default.Register<EmployeeAttendanceTimesViewModel>(true);
           
        }
        //public PersonalInformationViewModel PersonalInformationViewModel
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<PersonalInformationViewModel>();
        //    }
        //}
        public MainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public EmployeeViewModel EmployeeViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EmployeeViewModel>();
            }
        }
        public ContactInformationViewModel ContactInformationViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ContactInformationViewModel>();
            }
        }
        public EmployeeAttendanceTimesViewModel EmployeeAttendanceTimesViewModel
        {
            get
            {
                
                return ServiceLocator.Current.GetInstance<EmployeeAttendanceTimesViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}