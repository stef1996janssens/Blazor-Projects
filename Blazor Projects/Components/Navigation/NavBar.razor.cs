using Blazor_Projects.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Blazor_Projects.Components.Navigation;
public partial class NavBar : IDisposable
{
    [Parameter]
    public NavigationPositions Position { get; set; }

    [Parameter]
    public string Height { get; set; } = "auto";

    [Parameter]
    public string Width { get; set; } = "auto";

    [Parameter]
    public string Gap { get; set; } = "1em";

    [Parameter]
    public RenderFragment ChildContent { get; set; } = null!;

    [Parameter]
    public RenderFragment Body { get; set; } = null!;

    [Parameter]
    public string ContentPadding { get; set; } = string.Empty;

    [Parameter]
    public bool Development { get; set; } = false;

    [Inject]
    public IJSRuntime JS { get; set; } = null!;

    [Inject]
    private ILocalStorageService _localStorageService { get; set; } = null!;

    private string _navBarCss = string.Empty;
    private string _pageContentCss = string.Empty;
    private string _navSectionCss = string.Empty;


    public void Dispose()
    {
        throw new NotImplementedException();
    }

    protected override async Task OnInitializedAsync()
    {
        await SetNavBarPosition();
    }

    protected override async Task OnParametersSetAsync()
    {
        var position = JsonConvert.DeserializeObject<NavigationPositions>(await _localStorageService.Get("NavBarPosition")) ;
        await ChangeNavBarPostion(position);
    }


    private async Task<string> GetNavbarHeight()
    {
        var script = await JS.InvokeAsync<IJSObjectReference>("import", "./Components/Navigation/NavBar.razor.js");
        var height = await script.InvokeAsync<string>("getNavBarHeight");
        return height;
    }

    private async Task<string> GetNavbarWidth()
    {
        var script = await JS.InvokeAsync<IJSObjectReference>("import", "./Components/Navigation/NavBar.razor.js");
        var width = await script.InvokeAsync<string>("getNavBarWidth");
        return width;
    }

    private async Task ChangeNavBarPostion(NavigationPositions position)
    {
        switch (position)
        {
            case NavigationPositions.Top:
                Position = NavigationPositions.Top;
                break;
            case NavigationPositions.Right:
                Position = NavigationPositions.Right;
                break;
            case NavigationPositions.Bottom:
                Position = NavigationPositions.Bottom;
                break;
            case NavigationPositions.Left:
                Position = NavigationPositions.Left;
                break;
        }

        await SetNavBarPosition();
        await _localStorageService.Set("NavBarPosition", JsonConvert.SerializeObject(position));
    }

    private async Task SetNavBarPosition()
    {
        var contentPadding = string.Empty;
        switch (Position)
        {
            case NavigationPositions.Top:
                _navBarCss = $"top: 0; height: {Height}; width: 100dvw; flex-direction:row; gap:{Gap};";
                _navSectionCss = $"flex-direction: row; gap:{Gap};";
                contentPadding = string.IsNullOrEmpty(ContentPadding) ? $"{await GetNavbarHeight()}" : $"calc({await GetNavbarHeight()} + {ContentPadding}) {ContentPadding} {ContentPadding} {ContentPadding}";
                break;
            case NavigationPositions.Right:
                _navBarCss = $"right: 0; height: 100vh; width: {Width}; flex-direction:column; gap:{Gap};";
                contentPadding = string.IsNullOrEmpty(ContentPadding) ? $"{await GetNavbarWidth()}" : $"{ContentPadding} calc({await GetNavbarWidth()} + {ContentPadding})  {ContentPadding} {ContentPadding}";
                _navSectionCss = $"flex-direction: column; gap:{Gap};";
                break;
            case NavigationPositions.Bottom:
                _navBarCss = $"bottom: 0; height: {Height}; width: 100dvw; flex-direction:row; gap:{Gap};";
                contentPadding = string.IsNullOrEmpty(ContentPadding) ? $"{await GetNavbarHeight()}" : $"{ContentPadding} {ContentPadding} calc({await GetNavbarHeight()} + {ContentPadding}) {ContentPadding}";
                _navSectionCss = $"flex-direction: row; gap:{Gap};";
                break;
            case NavigationPositions.Left:
                _navBarCss = $"left: 0; height: 100dvh; width: {Width};flex-direction:column; gap:{Gap};";
                contentPadding = string.IsNullOrEmpty(ContentPadding) ? $"{await GetNavbarWidth()}" : $"{ContentPadding} {ContentPadding} {ContentPadding} calc({await GetNavbarWidth()} + {ContentPadding})";
                _navSectionCss = $"flex-direction: column; gap:{Gap};";
                break;
        }
        _pageContentCss = $"padding: {contentPadding}; height: 100dvh";
    }
}
