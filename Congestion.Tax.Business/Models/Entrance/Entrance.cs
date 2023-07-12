using Congestion.Tax.Business.Models.Vehicles;
using System;

namespace Congestion.Tax.Business.Models.Entrance
{
    public  class Entrance
    {
        public long Id { get; set; }
        public DateTime Datetime { get; set; }
        public string VehicleNumberPlate { get; set; }
        public VehicleType VehicleType { get; set; }
    }
}
