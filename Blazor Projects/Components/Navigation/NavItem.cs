using Microsoft.AspNetCore.Components;

namespace Blazor_Projects.Components.Navigation;

public class NavItem
{
    [Parameter]
    public string Type { get; set; }

    [Parameter]
    public string Text { get; set; }
}
