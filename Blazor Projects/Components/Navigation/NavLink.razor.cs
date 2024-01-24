using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor_Projects.Components.Navigation;
public partial class NavLink
{
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

    public delegate void OnClickNavLinkHandler();
    public event OnClickNavLinkHandler ClickNavLink;

    private async Task OnClickNavLink()
    {
        ClickNavLink?.Invoke();
    }
}
