using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolarPowerPlantAPI.Models
{
    public class TimeSeriesData
    {
        public int Id { get; set; }
        public int SolarPowerPlantId { get; set; }
        public DateTime Timestamp { get; set; }
        public double ProductionValue { get; set; }
    }
}
