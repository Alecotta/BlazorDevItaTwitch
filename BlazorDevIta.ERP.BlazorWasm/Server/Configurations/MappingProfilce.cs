using AutoMapper;
using BlazorDevIta.ERP.Business.Data;
using BlazorDevIta.Shared;

namespace BlazorDevIta.ERP.BlazorWasm.Server.Configurations
{
    public class MappingProfile : Profile
    {
        //Classe in cui vengono aggiunti i mapping.
        public MappingProfile()
        {
            CreateMap<WeatherForecast, WeatherForecastListItem>();
            CreateMap<WeatherForecast, WeatherForecastDetails>()
                //Permette il mapping al contrario
                .ReverseMap();
        }
    }
}
