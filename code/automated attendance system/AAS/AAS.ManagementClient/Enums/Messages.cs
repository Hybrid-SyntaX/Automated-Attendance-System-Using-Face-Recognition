using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAS.ManagementClient.Enums
{
    /// <summary>
    /// پیغام ها
    /// </summary>
    public enum Messages
    {
        /// <summary>
        /// به روز رسانی
        /// </summary>
        Update,

        /// <summary>
        /// بارگذاری مجدد
        /// </summary>
        Reset,

        /// <summary>
        /// حالت ویرایش
        /// </summary>
        ToggleEditMode,

        /// <summary>
        /// غیر فعال کردن حالت ویرایش
        /// </summary>
        DisableEditMode,

        /// <summary>
        /// فعال کردن حالت ویرایش
        /// </summary>
        EnableEditMode,

        /// <summary>
        /// درخواست یک نمونه از Employee
        /// </summary>
        EmployeeRequestsInstance,

        /// <summary>
        /// مخفی کردن EmployeeView
        /// </summary>
        HideEmployeeView,
    }
}
