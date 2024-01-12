namespace SolarPowerPlantAPI.Models
{
    public class ProductionData
    {
        public int Id { get; set; }
        public int PowerPlantId { get; set; } // Foreign key to link production data to a specific power plant
        public DateTime Timestamp { get; set; }
        public double ProductionValue { get; set; }
    }
}
