using Blazor_Projects.Components.Navigation.Enums;
using Microsoft.AspNetCore.Components;

namespace Blazor_Projects.Components.Navigation;
public partial class NavSection : BaseComponent
{
    [CascadingParameter]
    public NavigationPositions Position { get; set; }

    [CascadingParameter]
    public string NavLinkCss { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public string Gap { get; set; }

    [Parameter]
    public string Width { get; set; } = "auto";

    [Parameter]
    public string Height { get; set; } = "auto";

    [Parameter]
    public Spacing ItemSpacing { get; set; }


    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        await SetCssVariables();
    }

    private async Task SetCssVariables()
    {
        await SetCssVariable(CSSVariables.NavSection.Gap, Gap);
        StateHasChanged();
    }
}

