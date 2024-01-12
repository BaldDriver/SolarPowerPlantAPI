using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SolarPowerPlantAPI.Models;

namespace SolarPowerPlantAPI.Authentication
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<SolarPowerPlant> SolarPowerPlants { get; set; }

        public DbSet<ProductionData> ProductionDatas { get; set; }
    }
}
