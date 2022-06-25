using BlazorDevIta.ERP.Infrastructure.DataTypes;
using System.ComponentModel.DataAnnotations;

namespace BlazorDevIta.Shared
{
    public class WeatherForecastDetails : BaseDetails<int>
    {
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(50)]
        public string Summary { get; set; } = string.Empty;
        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}