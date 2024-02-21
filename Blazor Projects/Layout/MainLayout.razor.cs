using Blazor_Projects.Services;
using Microsoft.AspNetCore.Components;

namespace Blazor_Projects.Layout;

public partial class MainLayout
{
    [Inject]
    public StateService StateService { get; set; } = null!;
}
