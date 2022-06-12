using BlazorDevIta.Shared;

namespace BlazorDevIta.UI.Services;

public interface IDataServices
{
    Task<WeatherForecast[]?> GetWeatherForecastsAsync();
}
