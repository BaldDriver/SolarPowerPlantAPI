using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolarPowerPlantAPI.Authentication;
using SolarPowerPlantAPI.Models;

namespace SolarPowerPlantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolarPowerPlantController : ControllerBase
    {
        private readonly DataContext _context;

        public SolarPowerPlantController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var powerPlants = _context.SolarPowerPlants.ToList();
            return Ok(powerPlants);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var powerPlant = _context.SolarPowerPlants.Find(id);

            if (powerPlant == null)
                return NotFound();

            return Ok(powerPlant);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SolarPowerPlant powerPlant)
        {
            // Validation and error handling can be added here

            _context.SolarPowerPlants.Add(powerPlant);
            await _context.SaveChangesAsync();

            return Ok(new APIResponse { Message = "Solar power plant created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SolarPowerPlant updatedPowerPlant)
        {
            var powerPlant = _context.SolarPowerPlants.Find(id);

            if (powerPlant == null)
                return NotFound();

            // Validation and error handling can be added here

            powerPlant.Name = updatedPowerPlant.Name;
            powerPlant.InstalledPower = updatedPowerPlant.InstalledPower;
            powerPlant.DateOfInstallation = updatedPowerPlant.DateOfInstallation;
            powerPlant.Longitude = updatedPowerPlant.Longitude;
            powerPlant.Latitude = updatedPowerPlant.Latitude;

            await _context.SaveChangesAsync();

            return Ok(new APIResponse { Message = "Solar power plant updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var powerPlant = _context.SolarPowerPlants.Find(id);

            if (powerPlant == null)
                return NotFound();

            _context.SolarPowerPlants.Remove(powerPlant);
            await _context.SaveChangesAsync();

            return Ok(new APIResponse { Message = "Solar power plant deleted successfully" });
        }

        [HttpGet("{id}/production")]
        public IActionResult GetProductionData(int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string granularity)
        {
            var powerPlant = _context.SolarPowerPlants.Find(id);

            if (powerPlant == null)
                return NotFound();

            var productionData = _context.ProductionDatas
                .Where(p => p.PowerPlantId == id && p.Timestamp >= startDate && p.Timestamp <= endDate)
                .OrderBy(p => p.Timestamp)
                .ToList();

            // Optionally aggregate or filter data based on the specified granularity

            switch (granularity.ToLower())
            {
                case "day":
                    productionData = AggregateDataByDay(productionData);
                    break;
                case "hour":
                    productionData = AggregateDataByHour(productionData);
                    break;
                case "minute":
                    productionData = AggregateDataByMinute(productionData);
                    break;
                default:
                    // No aggregation, use the raw data
                    break;
            }

            return Ok(productionData);
        }

        private List<ProductionData> AggregateDataByDay(List<ProductionData> data)
        {
            return data.GroupBy(p => p.Timestamp.Date)
                       .Select(g => new ProductionData
                       {
                           Timestamp = g.Key,
                           ProductionValue = g.Sum(p => p.ProductionValue)
                       })
                       .ToList();
        }

        private List<ProductionData> AggregateDataByHour(List<ProductionData> data)
        {
            return data.GroupBy(p => new { p.Timestamp.Date, p.Timestamp.Hour })
                       .Select(g => new ProductionData
                       {
                           Timestamp = g.Key.Date.AddHours(g.Key.Hour),
                           ProductionValue = g.Sum(p => p.ProductionValue)
                       })
                       .ToList();
        }

        private List<ProductionData> AggregateDataByMinute(List<ProductionData> data)
        {
            return data.GroupBy(p => new { p.Timestamp.Date, p.Timestamp.Hour, p.Timestamp.Minute })
                       .Select(g => new ProductionData
                       {
                           Timestamp = g.Key.Date.AddHours(g.Key.Hour).AddMinutes(g.Key.Minute),
                           ProductionValue = g.Sum(p => p.ProductionValue)
                       })
                       .ToList();
        }
    }
}
