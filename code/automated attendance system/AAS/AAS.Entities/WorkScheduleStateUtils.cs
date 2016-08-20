using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAS.Entities
{
    /// <summary>
    /// کلاس کمکی برای تبدیل مقدار WorkSchedule.State به Boolean
    /// </summary>
    public static class WorkScheduleStateUtils
    {
        /// <summary>
        /// یک Extension Method برای State, برای تبدیل State به Boolean
        /// </summary>
        /// <param name="value">یک مقدار از State</param>
        /// <returns>bool</returns>
        public static bool ToBoolean(this WorkSchedule.State value)
        {
            switch (value)
            {
                case WorkSchedule.State.Work: return true;
                case WorkSchedule.State.Off: return false;
                default: throw new ArgumentOutOfRangeException("value");
            }
        }
    }
}
