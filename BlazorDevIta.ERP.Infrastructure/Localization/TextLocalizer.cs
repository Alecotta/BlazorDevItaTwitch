using Microsoft.Extensions.Localization;

namespace BlazorDevIta.ERP.Infrastructure.Localization;

public class TextLocalizer : ITextLocalizer
{
    //E' un dictionary
    private readonly IStringLocalizer _localizer;

    //Crea un localizzatore a partire dal file di risorse passato.
    public TextLocalizer(IStringLocalizerFactory factory, Type language)
    {
        _localizer = factory.Create(language);
    }

    public string Localize(string value)
    {
        //Value è la key del file di risorse. I due comandi sono equivalenti. Se non c'è la chiave, viene ritornata la chiave stessa.
        //var ret = _localizer[value];
        var ret = _localizer.GetString(value);
        return ret;
    }
}
