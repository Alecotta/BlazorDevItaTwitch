using BlazorDevIta.ERP.Infrastructure.DataTypes;
using BlazorDevIta.UI.Services;
using System.Net.Http.Json;

namespace BlazorDevIta.ERP.BlazorWasm.Client.Services;

public class DataServices<ListItemType, DetailsType, IdType>
    : IDataServices<ListItemType, DetailsType, IdType>
    where DetailsType : BaseDetails<IdType>
{
    private readonly HttpClient _http;
    private readonly IConfiguration _configuration;

    public DataServices(HttpClient http, IConfiguration configuration)
    {
        _http = http;
        _configuration = configuration;
    }

    public Task<List<ListItemType?>> GetAllAsync()
    {
        //var baseUrl = typeof(ListItemType).Name;
        var baseUrl = GetBaseUrl<ListItemType>();
        return _http.GetFromJsonAsync<List<ListItemType?>>(baseUrl)!;
    }

    public Task<DetailsType?> GetByIdAsync(IdType id)
    {
        var baseUrl = GetBaseUrl<DetailsType>();
        return _http.GetFromJsonAsync<DetailsType>($"{baseUrl}/{id}");
    }

    public Task CreateAsync(DetailsType details)
    {
        var baseUrl = GetBaseUrl<DetailsType>();
        return _http.PostAsJsonAsync(baseUrl, details);
    }

    public Task UpdateAsync(DetailsType details)
    {
        var baseUrl = GetBaseUrl<DetailsType>();
        return _http.PutAsJsonAsync($"{baseUrl}/{details.Id}", details);
    }

    public Task DeleteAsync(IdType id)
    {
        var baseUrl = GetBaseUrl<DetailsType>();
        return _http.DeleteAsync($"{baseUrl}/{id}");
    }

    private string GetBaseUrl<T>()
    {
        var baseUrl = _configuration[$"ApiUrls:{typeof(T).Name}"];
        if (baseUrl is null)
        {
            throw new Exception($"ApiUrls:{typeof(T).Name} not configured");
        }

        return baseUrl;
    }
}
