using Blazor_Projects.Services;
using Blazor_Projects.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor_Projects.Components;

public class BaseComponent : ComponentBase
{

    [Inject]
    protected ILocalStorageService LocalStorage { get; set; } = null!;

    [Inject]
    protected IJSRuntime JS { get; set; } = null!;

    [Inject]
    protected StateService StateService { get; set; } = null!;

    protected IJSObjectReference NavBarScript { get; set; } = null!;
    protected bool BaseComponentInitalized = false;


    protected override async Task OnInitializedAsync()
    {
        await ImportNavbarScript();
    }

    protected async Task SetCssVariable(string variableName, string value)
    {
        await NavBarScript.InvokeVoidAsync("setCssVariable", variableName, value);
    }

    protected async Task ImportNavbarScript()
    {
        NavBarScript = await JS.InvokeAsync<IJSObjectReference>("import", "./Components/Navigation/NavBar.razor.js");
        await Task.Delay(1);
    }

}
