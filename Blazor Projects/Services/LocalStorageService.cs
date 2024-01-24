using Blazor_Projects.Services.Interfaces;
using Microsoft.JSInterop;

namespace Blazor_Projects.Services;

public class LocalStorageService : ILocalStorageService
{
    private readonly IJSRuntime _js;

    public LocalStorageService(IJSRuntime jSRuntime)
    {
        _js = jSRuntime;
    }


    public async Task<string> Get(string key)
    {
        return await _js.InvokeAsync<string>("localStorage.getItem", key);
    }

    public async Task Set(string key, string value)
    {
        await _js.InvokeVoidAsync("localStorage.setItem", key, value);
    }

}
