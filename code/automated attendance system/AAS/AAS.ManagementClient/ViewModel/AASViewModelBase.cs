using AAS.ManagementClient.Enums;
using AAS.ManagementClient.Languages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;

namespace AAS.ManagementClient.ViewModel
{
    public class AASViewModelBase : ViewModelBase, INotifyDataErrorInfo
    {

        private Dictionary<string, List<string>> m_errors;
        
        
        private static string getNameFromExpression<T>(Expression<Func<T>> propertyExpression)
        {
            return ((MemberExpression)propertyExpression.Body).Member.Name;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// پاک کردن تمام خطا ها
        /// </summary>
        protected void clearErrors()
        {
            m_errors.Clear();

            if (HasErrors && m_errors != null & m_errors.Count > 0)
                foreach (KeyValuePair<string, List<string>> error in m_errors)
                //for (int i = 0; i < m_errors.Count; i++)
                {
                    RemoveError(error.Key);
                }

        }

        /// <summary>
        /// فارسی کردن MessageBox
        /// </summary>
        /// <param name="func">اعمالی که برای آن MessageBox فارسی می شود.</param>
        protected void localizeMessageBox(Action func)
        {
            ///[Localizing messagebox]
            MessageBoxManager.Yes = fa_IR.Yes;
            MessageBoxManager.No = fa_IR.No;
            MessageBoxManager.Register();
            func();
            MessageBoxManager.Unregister();
            ///[Localizing messagebox]
        }

        /// <summary>
        /// متد سازنده
        /// </summary>
        public AASViewModelBase()
        {
            m_errors = new Dictionary<string, List<string>>();
        }

        /// <summary>
        /// اعلام تغییر در تعداد خطا ها
        /// </summary>
        /// <typeparam name="T">نوع Property</typeparam>
        /// <param name="propertyExpression">عبارت حاوی Property</param>
        public void RaiseErrorsChanged<T>(Expression<Func<T>> propertyExpression)
        {
            RaiseErrorsChanged(getNameFromExpression<T>(propertyExpression));

        }
        
        /// <summary>
        /// اعلام تغییر در تعداد خطا ها
        /// </summary>
        /// <param name="propertyName">نام Property</param>
        public void RaiseErrorsChanged(string propertyName)
        {
            if (this.ErrorsChanged != null)
                this.ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));

            if (HasErrors)
                MessengerInstance.Send<Notification, MainViewModel>(new Notification()
                {
                    Message = fa_IR.FollowingFormContainsErrors,
                    Type = NotificationType.Error
                });
            else MessengerInstance.Send<Notification, MainViewModel>(null);
        }
        
        
        /// <summary>
        /// گرفتن خطاهای مربوط به Property خاص
        /// </summary>
        /// <param name="propertyName">نام Property</param>
        /// <returns>IEnumerable</returns>
        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName == null)
                return null;

            if (this.m_errors.ContainsKey(propertyName))
                return this.m_errors[propertyName];
            return null;
        }
        
        /// <summary>
        /// خطا وجود دارد؟
        /// </summary>
        public bool HasErrors
        {
            get { return (this.m_errors.Count > 0); }
        }

        
        /// <summary>
        /// اضافه کردن خطا
        /// </summary>
        /// <typeparam name="T">نوع Property</typeparam>
        /// <param name="propertyExpression">عبارت حاوی Property</param>
        /// <param name="error">خطا</param>
        public void AddError<T>(Expression<Func<T>> propertyExpression, string error)
        {
            AddError(getNameFromExpression(propertyExpression), error);

        }
        
        /// <summary>
        /// اضافه کردن خطا
        /// </summary>
        /// <param name="propertyName">نام Property</param>
        /// <param name="error">خطا</param>
        public void AddError(string propertyName, string error)
        {

            // Add error to list
            this.m_errors[propertyName] = new List<string>() { string.Format(error, getFieldLabel(propertyName)) };
            this.RaisePropertyChanged(() => this.HasErrors);
            this.RaiseErrorsChanged(propertyName);
        }
        
        /// <summary>
        ///حذف خطا
        /// </summary>
        /// <typeparam name="T">نوع Property</typeparam>
        /// <param name="propertyExpression">عبارت حاوی Property مورد نظر</param>
        public void RemoveError<T>(Expression<Func<T>> propertyExpression)
        {
         
            RemoveError(getNameFromExpression(propertyExpression));
        }
        
        /// <summary>
        /// حذف خطا
        /// </summary>
        /// <param name="propertyName">نام Property</param>
        public void RemoveError(string propertyName)
        {
            // remove error
            if (this.m_errors.ContainsKey(propertyName))
                this.m_errors.Remove(propertyName);
            this.RaiseErrorsChanged(propertyName);
            this.RaisePropertyChanged(() => this.HasErrors);
        }

        protected string getFieldLabel<T>(Expression<Func<T>> propertyExpression)
        {
            return getFieldLabel(getNameFromExpression(propertyExpression));
        }
        protected string getFieldLabel(string propertyName)
        {
            string propertyLabel = (string)App.Current.FindResource(propertyName);
            return propertyLabel;
        }

        protected void beginUpdatingTarget() { reverseBinding = true; }
        protected void endUpdatingTarget() { reverseBinding = false; }

        protected static bool reverseBinding = false;
        private ICommand m_explicitValidationCommand;
        /// <summary>
        /// عمل Validation صرح انجام می دهد.
        /// </summary>
        public ICommand ExplicitValidationCommand
        {
            get
            {
                if (m_explicitValidationCommand == null)
                {
                    m_explicitValidationCommand = new RelayCommand<TextBox>(explicitValidation);
                }
                return m_explicitValidationCommand;
            }
        }
        private void explicitValidation(TextBox textBox)
        {
            ///[Explicit validation]
            BindingExpression bindingExpression = textBox.GetBindingExpression(TextBox.TextProperty);

            if (!reverseBinding)
            {

                if (bindingExpression != null)
                {
                    bindingExpression.UpdateSource();
                }

                textBox.Text = (string)textBox.Tag;
                textBox.CaretIndex = textBox.Text.Length;
            }
            else bindingExpression.UpdateTarget();
            ///[Explicit validation]
        }

    }
}
