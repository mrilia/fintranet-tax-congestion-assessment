using System;

namespace Congestion.Tax.Business.Models.Tax
{
    public class TaxCalculationRequest
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string VehicleNumberPlate { get; set; } = string.Empty;
    }
}
