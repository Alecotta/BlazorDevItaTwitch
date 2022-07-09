using BlazorDevIta.UI.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System.Globalization;

namespace BlazorDevIta.UI.Configuration;

public static class Configuration
{
    //Con "add" viene utilizzato nella parte di servizi.
    public static IServiceCollection AddBlazorDevItaUI(this IServiceCollection services)
    {
        //Deve avere lo stesso lyfetime di chi la chiama.
        services.AddScoped<IConfirmService, ConfirmService>();
        return services;
    }

    //Con "use" viene utilizzato nella parte di app.
    public static async Task UseDefaultCulture(this WebAssemblyHost host)
    {
        //Con JS viene recuperato il servizio. 
        var jsInteropt = host.Services.GetRequiredService<IJSRuntime>();
        //Viene recuperato il valore dalla proprietà GET.
        var result = await jsInteropt.InvokeAsync<string>("blazorLanguage.get");
        CultureInfo culture;
        if (result is not null)
        {
            culture = new CultureInfo(result);
        }
        else
        {
            culture = new CultureInfo("en");
        }

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}
