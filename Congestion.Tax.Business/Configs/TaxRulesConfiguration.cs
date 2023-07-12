using System;
using System.Collections.Generic;

namespace Congestion.Tax.Business.Configs
{
    public class TaxRulesConfiguration
    {
        public List<DateTime> HolyDays { get; set; } = new List<DateTime>();
        public List<DayOfWeek> Weekends { get; set; } = new List<DayOfWeek>();
        public List<int> ExemptMonths { get; set; } = new List<int>();
        public int FreeDayCountBeforeHolyDays { get; set; } = 0;
        public List<string> TaxFreeVehicles { get; set; } = new List<string>();
        public List<TaxFee> TaxFees { get; set; } = new List<TaxFee>();
        public double MaxTaxPerDay { get; set; } = 0.0;
        public int MultipleTaxStationPassingDurationInMinute { get; set; } = 0;
    }
}
