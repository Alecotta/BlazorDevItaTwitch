using BlazorDevIta.ERP.BlazorServer.Data;
using BlazorDevIta.ERP.Infrastructure;
using BlazorDevIta.Shared;
using BlazorDevIta.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace BlazorDevIta.ERP.BlazorServer.Services;

public class DataServices : IDataServices
{
    private readonly IRepository<WeatherForecast, int> _repository;

    public DataServices(IRepository<WeatherForecast, int> repository)
    {
        _repository = repository;
    }

    public Task<List<WeatherForecastListItem?>> GetWeatherForecastsAsync()
    {
        //Dato il ToListAsync non serve il pattern async/await dato che ritorna un task.
        /*return _dbContext.WeatherForecasts
            //Non tiene traccia delle modifiche. Utile in operazioni di lettura.
            .AsNoTracking()
            .Select(x =>
            new WeatherForecastListItem()
            {
                Id = x.Id,
                Date = x.Date,
                TemperatureC = x.TemperatureC
            }).ToListAsync<WeatherForecastListItem?>();*/

        return _repository
            .GetAll()
            .Select(x =>
            new WeatherForecastListItem()
            {
                Id = x.Id,
                Date = x.Date,
                TemperatureC = x.TemperatureC
            }).ToListAsync<WeatherForecastListItem?>();
    }

    public async Task<WeatherForecastDetails?> GetWeatherForecastByIdAsync(int id)
    {
        /*return _dbContext.WeatherForecasts
            //Non tiene traccia delle modifiche. Utile in operazioni di lettura.
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x =>
                new WeatherForecastDetails()
                {
                    Id = x.Id,
                    Date = x.Date,
                    TemperatureC = x.TemperatureC,
                    Summary = x.Summary
                }).SingleOrDefaultAsync();*/

        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) return null;

        return new WeatherForecastDetails()
        {
            Id = entity.Id,
            Date = entity.Date,
            TemperatureC = entity.TemperatureC,
            Summary = entity.Summary
        };
    }

    public Task Create(WeatherForecastDetails details)
    {
        var entity = new WeatherForecast()
        {
            Date = details.Date,
            TemperatureC = details.TemperatureC,
            Summary = details.Summary
        };

        return _repository.Create(entity);

        /*_dbContext.WeatherForecasts.Add(entity);
        await _dbContext.SaveChangesAsync();
        //Così facendo indico di non tener traccia delle modifiche dell'entità.
        //Questo problema si ha con Blazor.Server dato che non utilizza richieste HTTP ma usa SignalR.
        _dbContext.Entry(entity).State = EntityState.Detached;*/
    }

    public Task Update(WeatherForecastDetails details)
    {
        var entity = new WeatherForecast()
        {
            Id = details.Id,
            Date = details.Date,
            TemperatureC = details.TemperatureC,
            Summary = details.Summary
        };

        return _repository.Update(entity);

        /*//Metodo che aggiorna il l'entità andandola prima a cercare.
        _dbContext.WeatherForecasts.Update(entity);
        await _dbContext.SaveChangesAsync();
        //Così facendo indico di non tener traccia delle modifiche dell'entità.
        //Questo problema si ha con Blazor.Server dato che non utilizza richieste HTTP ma usa SignalR.
        _dbContext.Entry(entity).State = EntityState.Detached;*/
    }

    public Task Delete(int id)
    {
        /*var entity = new WeatherForecast()
        {
            Id = id
        };
        //Così facendo indico di non tener traccia delle modifiche dell'entità.
        //Questo problema si ha con Blazor.Server dato che non utilizza richieste HTTP ma usa SignalR.
        _dbContext.WeatherForecasts.Remove(entity);
        return _dbContext.SaveChangesAsync();*/

        return _repository.Delete(id);
    }
}
