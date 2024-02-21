using Blazor_Projects.Components.Navigation.Enums;
using Blazor_Projects.Models;
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
    private List<Tile> _tiles = new List<Tile>();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await ImportSimulationScript();
        await InvokeGetTileCoordinates();
        await GetTilesFromLocalStorage();
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

    private async Task InvokeGetTileCoordinates()
    {
        await SimulationScript.InvokeVoidAsync("getTileCoordinates", DotNetObjectReference.Create(this));
    }

    private async Task GetTilesFromLocalStorage()
    {
        var tilesFromLocalStorage = await LocalStorage.Get("Tiles");
        if (!string.IsNullOrEmpty(tilesFromLocalStorage))
        {
            _tiles = JsonConvert.DeserializeObject<List<Tile>>(tilesFromLocalStorage) ?? new List<Tile>();
        }

    }

    [JSInvokable]
    public async Task GetTileCoordinates(List<Tile> tileCoordinates)
    {
        _tiles = tileCoordinates;
        await LocalStorage.Set("Tiles", JsonConvert.SerializeObject(_tiles));

    }

    [JSInvokable]
    public async Task UpdateTileCheckedState(Tile updatedTile)
    {
        var tile = _tiles.FirstOrDefault(x => x.X == updatedTile.X && x.Y == updatedTile.Y);
        tile.Checked = true;
    }
}
