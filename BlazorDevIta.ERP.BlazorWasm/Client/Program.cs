using BlazorDevIta.ERP.BlazorWasm.Client.Services;
using BlazorDevIta.UI;
using BlazorDevIta.UI.Configuration;
using BlazorDevIta.UI.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped(typeof(IDataServices<,,>), typeof(DataServices<,,>));

//Aggiunto dalla libreria
builder.Services.AddBlazorDevItaUI();

await builder.Build().RunAsync();
