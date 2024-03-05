using Blazor_Projects.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Blazor_Projects.Pages;
public partial class Simulation : BasePage
{
    private IJSObjectReference SimulationScript { get; set; } = null!;

    private int _amountOfColumns = 10;
    private int _maxAmountOfColumns = 100;
    private int _amountOfRows = 10;
    private int _maxAmountOfRows = 100;
    private int _tileSize = 20;
    private List<Tile> _tiles = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await ImportSimulationScript();
        await InitializeDotNetInstanceOnJS();
        await SetGridParametersFromLocalStorage();
        await InitializeCanvas();
        await DrawGrid();
    }

    private async Task ImportSimulationScript()
    {
        SimulationScript = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/Simulation.razor.js");
        await Task.Delay(1);
    }

    private async Task SetJSVariable(ChangeEventArgs e, string functionName, string variableName)
    {
        var value = Convert.ToInt32(e.Value);
        await SimulationScript.InvokeVoidAsync(functionName, value);

        var gridParameters = await GetGridParametersFromLocalStorage();

        var parameter = gridParameters.GetType().GetProperty(variableName);
        parameter?.SetValue(gridParameters, value);

        await LocalStorage.Set("GridParameters", JsonConvert.SerializeObject(gridParameters));
        await GenerateGrid();
        StateHasChanged();
    }

    private async Task InitializeDotNetInstanceOnJS()
    {
        await SimulationScript.InvokeVoidAsync("initializeDotNetInstance", DotNetObjectReference.Create(this));
    }

    private async Task SetGridParametersFromLocalStorage()
    {
        var gridParameters = await GetGridParametersFromLocalStorage();

        if (gridParameters != null)
        {
            _amountOfColumns = gridParameters.AmountOfColumns;
            _amountOfRows = gridParameters.AmountOfRows;
            _tileSize = gridParameters.TileSize;
        }

        await SimulationScript.InvokeVoidAsync("setTileSize", _tileSize);
        await SimulationScript.InvokeVoidAsync("setAmountOfColumns", _amountOfColumns);
        await SimulationScript.InvokeVoidAsync("setAmountOfRows", _amountOfRows);
    }

    private async Task<GridParameter?> GetGridParametersFromLocalStorage()
    {
        var gridParametersFromLocalStorage = await LocalStorage.Get("GridParameters");
        if (!string.IsNullOrEmpty(gridParametersFromLocalStorage))
        {
            return JsonConvert.DeserializeObject<GridParameter>(gridParametersFromLocalStorage);
        }

        return new GridParameter { AmountOfColumns = _amountOfColumns, AmountOfRows = _amountOfRows, TileSize = _tileSize };
    }

    private async Task DrawGrid()
    {
        var tilesFromLocalStorage = await LocalStorage.Get("Tiles");
        if (!string.IsNullOrEmpty(tilesFromLocalStorage))
        {
            _tiles = JsonConvert.DeserializeObject<List<Tile>>(tilesFromLocalStorage) ?? new List<Tile>();
            if (_tiles.Any())
                await DrawTiles(_tiles);
            else
                await GenerateGrid();
        }
        else
            await GenerateGrid();

    }

    private async Task DrawTiles(List<Tile> tiles)
    {
        foreach (var tile in tiles)
        {
            await SimulationScript.InvokeVoidAsync("drawRectangle", tile.X, tile.Y, _tileSize, _tileSize, tile.LineColor, tile.LineWidth, tile.FillColor);
        }

        StateHasChanged();
    }

    private async Task GenerateGrid()
    {
        await SimulationScript.InvokeVoidAsync("generateGrid");
    }

    private async Task InitializeCanvas()
    {
        await SimulationScript.InvokeVoidAsync("initializeCanvas");
    }
    
    [JSInvokable]
    public async Task SaveTiles(List<Tile> tiles)
    {
        _tiles = tiles;
        await LocalStorage.Set("Tiles", JsonConvert.SerializeObject(_tiles));
        await DrawTiles(_tiles);
    }

    [JSInvokable]
    public async Task UpdateTile(string updatedTileAsJson)
    {
        var updatedTile = JsonConvert.DeserializeObject<Tile>(updatedTileAsJson);
        
        if (updatedTile == null) return;

        var tile = _tiles.FirstOrDefault(x => x.X == updatedTile.X && x.Y == updatedTile.Y);
        if (tile == null) return;

        tile.Checked = updatedTile.Checked;
        tile.FillColor = "gray";

        await SimulationScript.InvokeVoidAsync("redrawTile", tile);
        await LocalStorage.Set("Tiles", JsonConvert.SerializeObject(_tiles));
    }
}
