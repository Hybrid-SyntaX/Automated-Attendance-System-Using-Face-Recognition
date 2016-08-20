using System;
namespace AAS.Entities.Interfaces
{
    /// <summary>
    /// اینترفیس IContactInformation
    /// </summary>
    public interface IContactInformation
    {
        /// <summary>
        /// آدرس
        /// </summary>
        string Address { get; set; }

        /// <summary>
        /// شماره تلفن همراه
        /// </summary>
        string CellphoneNumber { get; set; }

        /// <summary>
        /// پست الکترونیک
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// کارمند مربوطه
        /// </summary>
        Employee Employee { get; set; }

        /// <summary>
        /// شناسه داخلی قابل استفاده در پایگاه داده
        /// </summary>
        Guid ID { get; set; }

        /// <summary>
        /// برچسب
        /// </summary>
        string Label { get; set; }

        /// <summary>
        /// شماره تلفن
        /// </summary>
        string PhoneNumber { get; set; }

        /// <summary>
        /// کد پستی
        /// </summary>
        string PostalCode { get; set; }
    }
}
