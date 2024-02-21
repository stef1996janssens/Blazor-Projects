using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Blazor_Projects.Pages;
public partial class Simulation : BasePage
{
    private IJSObjectReference SimulationScript { get; set; } = null!;

    private int _amountOfColumns = 0;
    private int _maxAmountOfColumns = 100;
    private int _amountOfRows = 0;
    private int _maxAmountOfRows = 100;
    private int _tileSize = 0;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await ImportSimulationScript();

    }


    private async Task ImportSimulationScript()
    {
        SimulationScript = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/Simulation.razor.js");
        await Task.Delay(1);
    }

    private async Task SetJSVariable(ChangeEventArgs e, string functionName)
    {
        var value = Convert.ToInt32(e.Value);
        await SimulationScript.InvokeVoidAsync(functionName, value);
        StateHasChanged();
    }
}
