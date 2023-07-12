namespace Congestion.Tax.API.Models
{
    public class FilterDto
    {
        public DateTime From { get; set; }
        public DateTime To{ get; set; }
        public string VehicleNumberPlate { get; set; } = string.Empty;

    }
}
