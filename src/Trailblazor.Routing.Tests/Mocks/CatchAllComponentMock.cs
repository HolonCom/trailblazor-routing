using Microsoft.AspNetCore.Components;

namespace Trailblazor.Routing.Tests.Mocks;

internal sealed class CatchAllComponentMock : ComponentBase
{
    [Parameter]
    public string? Path { get; set; }
}
