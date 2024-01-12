namespace SolarPowerPlantAPI.Models
{
    public class SolarPowerPlant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double InstalledPower { get; set; }
        public DateTime DateOfInstallation { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
