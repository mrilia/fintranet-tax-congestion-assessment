using System;

namespace Congestion.Tax.Business.Configs
{
    public class TaxFee
    {
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
        public double Fee { get; set; }
    }
}
