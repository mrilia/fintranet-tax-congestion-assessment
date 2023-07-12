namespace Congestion.Tax.API.Models
{
    public class TaxCalculationRequestDto
    {
        public DateTime From { get; set; }
        public DateTime To{ get; set; }
        public string VehicleNumberPlate { get; set; } = string.Empty;

    }
}
