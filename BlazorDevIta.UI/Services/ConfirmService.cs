using Microsoft.JSInterop;

namespace BlazorDevIta.UI.Services;

public class ConfirmService : IAsyncDisposable, IConfirmService
{
    private readonly IJSRuntime _jsRuntime;
    private IJSObjectReference? module = null;

    public ConfirmService(IJSRuntime jSRuntime)
    {
        _jsRuntime = jSRuntime;
    }

    public async Task Init()
    {
        module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/BlazorDevIta.UI/confirm.js");
    }

    public async Task ShowConfirm(string confirmId)
    {
        //Show Confirm. Funzione che si chiama showConfirm e parametri.
        if (module is not null)
        {
            await module.InvokeVoidAsync("showConfirm", confirmId);
        }
    }

    public async Task HideConfirm(string confirmId)
    {
        //Hide Confirm.
        if (module is not null)
        {
            await module.InvokeVoidAsync("hideConfirm", confirmId);
        }
    }

    //Metodo necessario per fare la dispose degli oggetti usati da JSInteropt.
    public async ValueTask DisposeAsync()
    {
        if (module is not null)
        {
            await module.DisposeAsync();
        }
    }
}
