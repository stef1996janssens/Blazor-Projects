using Blazor_Projects.Components.Navigation.Enums;
using Microsoft.AspNetCore.Components;

namespace Blazor_Projects.Components.Navigation;
public partial class NavLink
{
    [CascadingParameter]
    public NavigationPositions Position { get; set; }

    [Parameter]
    public required string Text { get; set; }
    [Parameter]
    public required string Href { get; set; }
    [Parameter]
    public string? Icon { get; set; }
    [Parameter]
    public string IconSize { get; set; } = string.Empty;
    [Parameter]
    public string GapWidth { get; set; } = "1em";
    [Parameter]
    public string Colour { get; set; } = "#fff";


    public delegate void OnClickNavLinkHandler();
    public event OnClickNavLinkHandler ClickNavLink;

    private async Task OnClickNavLink()
    {
        ClickNavLink?.Invoke();
    }
}
