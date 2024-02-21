using Blazor_Projects.Components.Navigation.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Blazor_Projects.Components.Navigation;
public partial class NavBar : BaseComponent, IDisposable
{
    /// <summary>
    /// Sets the color of the background to a valid cssvalue.
    /// </summary>
    [Parameter] public string BackgroundColor { get; set; } = "#ffffff";
    /// <summary>
    /// Sets the padding of the pagebody to a valid cssvalue.
    /// </summary>
    [Parameter] public string ContentPadding { get; set; } = "0px";
    /// <summary>
    /// Sets the padding of the navbar to a valid cssvalue.
    /// </summary>
    [Parameter] public string Padding { get; set; } = "0px";
    /// <summary>
    /// Shows UI to test the parameters.
    /// </summary>
    [Parameter] public bool Development { get; set; } = false;
    /// <summary>
    /// Sets the gap between navSections to a valid cssvalue.
    /// </summary>
    [Parameter] public string Gap { get; set; } = "1em";
    /// <summary>
    /// Sets the height of the navbar to a valid cssvalue.
    /// </summary>
    [Parameter] public string Height { get; set; } = "auto";
    /// <summary>
    /// Sets the width of the navbar to a valid cssvalue.
    /// </summary>
    [Parameter] public string Width { get; set; } = "auto";
    /// <summary>
    /// Places navbar top/right/bottom/left by using the NavigationPositions enum.
    /// </summary>
    [Parameter] public required NavigationPositions Position { get; set; }
    [Parameter] public Spacing Spacing { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; } = null!;
    /// <summary>
    /// Required: this is the body that needs to be rendered on the pagebody.
    /// </summary>
    [Parameter] public required RenderFragment Body { get; set; } = null!;

    private string _positionClass = string.Empty;
    private string _navSectionCss = string.Empty;

    private bool _doneRendering = false;
    private bool _developmentActive = true;

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        var positionFromLocalStorage = await LocalStorage.Get("NavBarPosition");
        if (!string.IsNullOrEmpty(positionFromLocalStorage))
        {
            Position = JsonConvert.DeserializeObject<NavigationPositions>(positionFromLocalStorage);
        }

        await SetCssVariables();
        await ChangeNavBarPostion(Position);
    }

    private async Task SetCssVariables()
    {
        await SetCssVariable(CSSVariables.NavBar.BackgroundColor, BackgroundColor);
        await SetCssVariable(CSSVariables.NavBar.Padding, Padding);
        _doneRendering = true;
        StateHasChanged();
    }

    private async Task<string> GetNavbarHeight()
    {
        var height = await NavBarScript.InvokeAsync<string>("getNavBarHeight");
        return height;
    }

    private async Task<string> GetNavbarWidth()
    {
        var width = await NavBarScript.InvokeAsync<string>("getNavBarWidth");
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

        for (int i = 0; i < 2; i++)
        {
            await SetNavBarPosition();
        }

        await LocalStorage.Set("NavBarPosition", JsonConvert.SerializeObject(position));
    }

    private async Task SetNavBarPosition()
    {
        var contentPadding = string.Empty;
        switch (Position)
        {
            case NavigationPositions.Top:
                await SetNavPostionTop();
                await SetContentPaddingNavBarTop();
                break;
            case NavigationPositions.Right:
                await SetNavPostionRight();
                await SetContentPaddingNavBarRight();
                break;
            case NavigationPositions.Bottom:
                await SetNavPostionBottom();
                await SetContentPaddingNavBarBottom();
                break;
            case NavigationPositions.Left:
                await SetNavPostionLeft();
                await SetContentPaddingNavBarLeft();
                break;
        }

        await SetCssVariable(CSSVariables.NavBar.Gap, Gap);
    }

    private async Task SetNavPostionTop()
    {
        _positionClass = CSSClasses.NavPositionTop;
        await SetCssVariable(CSSVariables.NavBar.Height, Height);
        await SetCssVariable(CSSVariables.NavBar.Width, "100dvw");
        _navSectionCss = CSSClasses.FlexRow;

        StateHasChanged();
    }

    private async Task SetNavPostionRight()
    {
        _positionClass = CSSClasses.NavPositionRight;
        await SetCssVariable(CSSVariables.NavBar.Height, "100dvh");
        await SetCssVariable(CSSVariables.NavBar.Width, Width);

        _navSectionCss = CSSClasses.FlexColumn;
        StateHasChanged();
    }

    private async Task SetNavPostionBottom()
    {
        _positionClass = CSSClasses.NavPositionBottom;
        await SetCssVariable(CSSVariables.NavBar.Height, Height);
        await SetCssVariable(CSSVariables.NavBar.Width, "100dvw");
        _navSectionCss = CSSClasses.FlexRow;

        _navSectionCss = CSSClasses.FlexRow;
        StateHasChanged();
    }

    private async Task SetNavPostionLeft()
    {
        _positionClass = CSSClasses.NavPositionLeft;
        await SetCssVariable(CSSVariables.NavBar.Height, "100dvh");
        await SetCssVariable(CSSVariables.NavBar.Width, Width);

        _navSectionCss = CSSClasses.FlexColumn;
        StateHasChanged();
    }

    private async Task SetContentPaddingNavBarTop()
    {
        if (string.IsNullOrEmpty(ContentPadding))
            await SetContentPaddingVariables(await GetNavbarHeight(), null, null, null);
        else
            await SetContentPaddingVariables($"calc({await GetNavbarHeight()} + {ContentPadding} + {Padding})", ContentPadding, ContentPadding, ContentPadding);
    }

    private async Task SetContentPaddingNavBarRight()
    {
        if (string.IsNullOrEmpty(ContentPadding))
            await SetContentPaddingVariables(null, await GetNavbarWidth(), null, null);
        else
            await SetContentPaddingVariables(ContentPadding, $"calc({await GetNavbarWidth()} + {ContentPadding} + {Padding})", ContentPadding, ContentPadding);
    }

    private async Task SetContentPaddingNavBarBottom()
    {
        if (string.IsNullOrEmpty(ContentPadding))
            await SetContentPaddingVariables(null, null, await GetNavbarHeight(), null);
        else
            await SetContentPaddingVariables(ContentPadding, ContentPadding, $"calc({await GetNavbarHeight()} + {ContentPadding} + {Padding})", ContentPadding);
    }

    private async Task SetContentPaddingNavBarLeft()
    {
        if (string.IsNullOrEmpty(ContentPadding))
            await SetContentPaddingVariables(null, null, null, await GetNavbarWidth());
        else
            await SetContentPaddingVariables(ContentPadding, ContentPadding, ContentPadding, $"calc({await GetNavbarWidth()} + {ContentPadding} + {Padding})");
    }

    private async Task SetContentPaddingVariables(string? paddingTop = null, string? paddingRight = null, string? paddingBottom = null, string? paddingLeft = null)
    {
        await SetCssVariable(CSSVariables.Content.PaddingTop, paddingTop ?? "0px");
        await SetCssVariable(CSSVariables.Content.PaddingRight, paddingRight ?? "0px");
        await SetCssVariable(CSSVariables.Content.PaddingBottom, paddingBottom ?? "0px");
        await SetCssVariable(CSSVariables.Content.PaddingLeft, paddingLeft ?? "0px");

    }
}
