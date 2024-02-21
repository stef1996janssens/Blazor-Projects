using Microsoft.AspNetCore.Components;

namespace Blazor_Projects.Components;
public partial class ExampleCard : BaseComponent
{
    [Parameter]
    public string Text { get; set; } = "Placeholder";
    [Parameter]
    public string? Image { get; set; }
    [Parameter]
    public string Width { get; set; } = "100px";
    [Parameter]
    public string Height { get; set; } = "100px";
    [Parameter]
    public string BorderRadius { get; set; } = "auto";
    [Parameter]
    public string BorderStyle { get; set; } = "auto";
    [Parameter]
    public string BorderWidth { get; set; } = "auto";
    [Parameter]
    public string BorderColor { get; set; } = "#fff";
    [Parameter]
    public string Color { get; set; } = "#000";
    [Parameter]
    public string BackgroundColor { get; set; } = "#fff";

    protected override Task OnParametersSetAsync()
    {
        return base.OnParametersSetAsync();
    }
}
