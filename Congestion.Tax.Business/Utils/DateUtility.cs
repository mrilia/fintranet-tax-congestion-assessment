using Congestion.Tax.Business.Configs;
using System;
using System.Linq;

namespace Congestion.Tax.Business.Utils
{
    public static class DateUtility
    {
        static TaxRulesConfiguration _taxConfigs = TaxConfigurationUtility.GetConfigs();

        public static bool IsDayBeforeOfPublicHolyday(DateTime date)
        {
            return _taxConfigs.HolyDays?.Any(a => a.AddDays(-1).DayOfYear == date.DayOfYear) ?? false;
        }

        public static bool IsPublicHolyDay(DateTime date)
        {
            return _taxConfigs.HolyDays?.Contains(date) ?? false;
        }

        public static bool IsInFreeMonth(DateTime date)
        {
            return _taxConfigs.ExemptMonths?.Contains(date.Month) ?? false;
        }

        public static bool IsWeekend(DateTime date)
        {
            return _taxConfigs.Weekends?.Contains(date.DayOfWeek) ?? false;
        }
    }
}
