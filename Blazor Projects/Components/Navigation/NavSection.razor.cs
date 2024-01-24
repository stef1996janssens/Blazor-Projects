using Microsoft.AspNetCore.Components;

namespace Blazor_Projects.Components.Navigation;
public partial class NavSection
{
    [CascadingParameter]
    public NavigationPositions Position { get; set; }

    [CascadingParameter]
    public string NavLinkCss { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public string Gap { get; set; }

    private string _navLinksCss = string.Empty;


    //protected override void OnParametersSet()
    //{
    //    StateHasChanged();
    //}
}
