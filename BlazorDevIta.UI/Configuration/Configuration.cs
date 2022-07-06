using BlazorDevIta.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorDevIta.UI.Configuration;

public static class Configuration
{
    public static IServiceCollection AddBlazorDevItaUI(this IServiceCollection services)
    {
        //Deve avere lo stesso lyfetime di chi la chiama.
        services.AddScoped<IConfirmService, ConfirmService>();
        return services;
    }
}
