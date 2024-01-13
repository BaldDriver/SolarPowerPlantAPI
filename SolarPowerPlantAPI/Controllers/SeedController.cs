using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SolarPowerPlantAPI.Authentication;
using SolarPowerPlantAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SolarPowerPlantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly DataContext _context;

        public SeedController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("seed-data")]
        public IActionResult SeedData()
        {
            foreach (var solarPowerPlant in _context.SolarPowerPlants)
            {
                var random = new Random();
                var startDate = DateTime.Now.AddMonths(-12);
                var timeSeriesData = new List<TimeSeriesData>();

                for (var date = startDate; date <= DateTime.Now; date = date.AddHours(1))
                {
                    var productionValue = random.NextDouble() * 100; 

                    timeSeriesData.Add(new TimeSeriesData
                    {
                        SolarPowerPlantId = solarPowerPlant.Id,
                        Timestamp = date,
                        ProductionValue = productionValue
                    });
                }

                _context.TimeSeriesDatas.AddRange(timeSeriesData);
                _context.SaveChanges();
            }

            return Ok("Seed data generated successfully!");
        }
    }
}
