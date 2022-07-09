using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;

namespace BlazorDevIta.UI.Components;

public partial class LanguageSelector
{
    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    //Culture che si scelgono di mettere a disposizione.
    CultureInfo[] cultures = new[]
    {
        new CultureInfo("en"),
        new CultureInfo("it"),
        new CultureInfo("fr"),
    };

    //Property che va sul local storage.
    CultureInfo Culture
    {
        get => CultureInfo.CurrentCulture;
        set
        {
            if (CultureInfo.CurrentCulture != value)
            {
                var js = (IJSInProcessRuntime)JSRuntime;
                js.InvokeVoid("blazorLanguage.set", value.Name);
                //Viene ricaricata la pagina (forceLoad indica di fare il ricaricamento della pagina).
                //Il NavigationManager permette di interfacciarsi con il motore di routing.
                //Basandosi sui file .Resx l'aggiornamento della pagina è necessario.
                NavManager.NavigateTo(NavManager.Uri, forceLoad: true);
            }
        }
    }
}
