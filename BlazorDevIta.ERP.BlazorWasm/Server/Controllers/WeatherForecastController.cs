using AutoMapper;
using BlazorDevIta.ERP.Business.Data;
using BlazorDevIta.ERP.Infrastructure;
using BlazorDevIta.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorDevIta.ERP.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : CRUDController<WeatherForecastListItem, WeatherForecastDetails, int, WeatherForecast>
    {
        public WeatherForecastController(
            IMapper mapper,
            ILogger<WeatherForecastController> logger,
            IRepository<WeatherForecast, int> repository)
            : base(mapper, logger, repository)
        {
        }
    }
}