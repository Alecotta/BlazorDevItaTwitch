using Microsoft.EntityFrameworkCore;

namespace BlazorDevIta.ERP.Business.Data
{
    public class ERPDbContext : DbContext
    {
        public ERPDbContext(DbContextOptions otp)
                : base(otp) { }

        public DbSet<WeatherForecast> WeatherForecasts => Set<WeatherForecast>();
    }
}
