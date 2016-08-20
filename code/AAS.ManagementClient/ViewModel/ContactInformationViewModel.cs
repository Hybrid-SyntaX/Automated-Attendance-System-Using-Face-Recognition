using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using AAS.Entities;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using AAS.ManagementClient.Languages;
using AAS.Entities.Exceptions;
using AAS.Entities.Interfaces;
using GalaSoft.MvvmLight.Ioc;
using AAS.ManagementClient.Enums;
namespace AAS.ManagementClient.ViewModel
{
    /// <summary>
    /// ContactInformationViewModel
    /// </summary>
    public class ContactInformationViewModel : AASViewModelBase, IContactInformation
    {
        private ContactInformation m_contactInformation;
        public ContactInformation ContactInformation
        {
            set
            {
                m_contactInformation = value;
                RaisePropertyChanged(() => ContactInformation);
            }
            get
            {
                return m_contactInformation;
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

        public ContactInformationViewModel(ContactInformation contactInformation)
        {

            ContactInformation = contactInformation;
    
            registerForMessages();

        }
        [PreferredConstructor]
        public ContactInformationViewModel()
        {

            Messenger.Default.Register<ContactInformation>(this, (message) => ContactInformation = message);


            if (ContactInformation == null)
                ContactInformation = new ContactInformation();


            registerForMessages();


        }

     
        private void registerForMessages()
        {
            Messenger.Default.Register<Messages>(this, (message) =>
            {
                if (message == Messages.Reset)
                {
                    reset();
                }
                if (message == Messages.ToggleEditMode)
                {
                    EditMode = !EditMode;
                }
                if (message == Messages.EnableEditMode)
                {
                    EditMode = true;
                }
                if (message == Messages.DisableEditMode)
                {
                    EditMode = false;
                }
            });
        }

        /// <summary>
        /// برچسب
        /// </summary>
        /// <value>m_contactInformation.Label</value>
        public string Label
        {
            set
            {
                try
                {

                    ContactInformation.Label = value;
                    RemoveError(() => Label);
                }
                catch (RequiredFieldException)
                {
                    AddError(() => Label, string.Format(fa_IR.RequiredField, getFieldLabel(() => Label)));
                }
                catch (InvalidFormatException)
                {
                    AddError(() => Label, string.Format(fa_IR.InvalidNumber, getFieldLabel(() => Label)));
                }
                catch (Exception)
                {
                    //TODO: log this
                    AddError(() => Label, string.Format(fa_IR.UnknownError, getFieldLabel(() => Label)));
                }
                finally
                {

                    RaisePropertyChanged(() => this.Label);
                    RaiseErrorsChanged(() => this.Label);
                }
            }
            get
            {
                return ContactInformation.Label;
            }
        }

        /// <summary>
        /// شماره تلفن
        /// </summary>
        /// /// <value>ContactInformation.PhoneNumber</value>
        public string PhoneNumber
        {
            set
            {
                try
                {
                    ContactInformation.PhoneNumber = value;
                    RemoveError(() => this.PhoneNumber);
                }
                catch (RequiredFieldException)
                {
                    AddError(() => PhoneNumber, string.Format(fa_IR.RequiredField, getFieldLabel(() => PhoneNumber)));
                }
                catch (InvalidFormatException)
                {
                    AddError(() => PhoneNumber, string.Format(fa_IR.InvalidPhoneNumber, getFieldLabel(() => PhoneNumber)));
                }
                catch (Exception)
                {
                    //TODO: log this
                    AddError(() => PhoneNumber, string.Format(fa_IR.UnknownError, getFieldLabel(() => PhoneNumber)));
                }
                finally
                {

                    RaisePropertyChanged(() => this.PhoneNumber);
                    RaiseErrorsChanged(() => this.PhoneNumber);
                }
            }
            get
            {
                return ContactInformation.PhoneNumber;
            }
        }

        /// <summary>
        /// شماره همراه
        /// </summary>
        /// <value>ContactInformation.CellphoneNumber</value>
        public string CellphoneNumber
        {
            set
            {
                try
                {
                    ContactInformation.CellphoneNumber = value;
                    RemoveError(() => this.CellphoneNumber);
                }
                catch (RequiredFieldException)
                {
                    AddError(() => CellphoneNumber, string.Format(fa_IR.RequiredField, getFieldLabel(() => CellphoneNumber)));
                }
                catch (InvalidFormatException)
                {
                    AddError(() => CellphoneNumber, string.Format(fa_IR.InvalidCellphoneNumber, getFieldLabel(() => CellphoneNumber)));
                }
                catch (Exception)
                {
                    //TODO: log this
                    AddError(() => CellphoneNumber, string.Format(fa_IR.UnknownError, getFieldLabel(() => CellphoneNumber)));
                }
                finally
                {

                    RaisePropertyChanged(() => this.CellphoneNumber);
                    RaiseErrorsChanged(() => CellphoneNumber);
                }
            }
            get
            {
                return ContactInformation.CellphoneNumber;
            }
        }

        /// <summary>
        /// پست الکترونیک
        /// </summary>
        /// <value>ContactInformation.Email</value>
        public string Email
        {
            set
            {
                try
                {
                    ContactInformation.Email = value;
                    RemoveError(() => this.Email);
                }
                catch (RequiredFieldException)
                {
                    AddError(() => Email, string.Format(fa_IR.RequiredField, getFieldLabel(() => Email)));
                }
                catch (InvalidFormatException)
                {
                    AddError(() => Email, string.Format(fa_IR.InvalidEmailFormat));
                }
                catch (Exception)
                {
                    //TODO: log this
                    AddError(() => Email, string.Format(fa_IR.UnknownError, getFieldLabel(() => Email)));
                }
                finally
                {

                    RaisePropertyChanged(() => this.Email);
                    RaiseErrorsChanged(() => this.Email);
                }
            }
            get
            {
                return ContactInformation.Email;
            }
        }

        /// <summary>
        /// آدرس
        /// </summary>
        /// <value>ContactInformation.Address</value>
        public string Address
        {
            set
            {
                try
                {
                    ContactInformation.Address = value;
                    RemoveError(() => this.Address);
                }
                catch (RequiredFieldException)
                {
                    AddError(() => Address, fa_IR.RequiredField);
                }
                catch (InputTooShortException)
                {
                    AddError(() => Address, fa_IR.InputTooShort);
                }
                catch (Exception)
                {
                    //TODO: log this
                    AddError(() => Address, fa_IR.UnknownError);
                }
                finally
                {

                    RaisePropertyChanged(() => this.Address);
                    RaiseErrorsChanged(() => this.Address);
                }
            }
            get
            {
                return ContactInformation.Address;
            }
        }

        /// <summary>
        /// کد پستی
        /// </summary>
        /// <value>ContactInformation.PostalCode</value>
        public string PostalCode
        {
            set
            {
                try
                {
                    ContactInformation.PostalCode = value;
                    RemoveError(() => this.PostalCode);
                }
                catch (RequiredFieldException)
                {
                    AddError(() => PostalCode, string.Format(fa_IR.RequiredField, getFieldLabel(() => PostalCode)));
                }
                catch (InvalidFormatException)
                {
                    AddError(() => PostalCode, string.Format(fa_IR.InvalidPostalCodeFormat));
                }
                catch (Exception)
                {
                    //TODO: log this
                    AddError(() => PostalCode, string.Format(fa_IR.UnknownError, getFieldLabel(() => PostalCode)));
                }
                finally
                {
                    RaisePropertyChanged(() => this.PostalCode);
                    RaiseErrorsChanged(() => this.PostalCode);
                }
            }
            get
            {
                return ContactInformation.PostalCode;
            }
        }

        /// <summary>
        /// شناسنه داخلی در ViewModel پشتی بانی نمی شود
        /// </summary>
        public Guid ID
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// کارمند
        /// </summary>
        /// <value>ContactInformation.Employee</value>
        public Employee Employee
        {
            set
            {
                ContactInformation.Employee = value;
                RaisePropertyChanged(() => Employee);
            }
            get
            {
                return this.ContactInformation.Employee;
            }
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
            clearErrors();
            ContactInformation = new ContactInformation();
            RaisePropertyChanged(() => this.Label);
            RaisePropertyChanged(() => this.PhoneNumber);
            RaisePropertyChanged(() => this.CellphoneNumber);
            RaisePropertyChanged(() => this.Email);
            RaisePropertyChanged(() => this.Address);
            RaisePropertyChanged(() => this.PostalCode);
            RaisePropertyChanged(() => Employee);
            Messenger.Default.Send<ContactInformation, EmployeeViewModel>(ContactInformation);
        }

        public override string ToString()
        {
            return ContactInformation.ToString();
        }


    }
}
