using Trailblazor.Routing.App.WASM.Pages;
using Trailblazor.Routing.Configuration;

namespace Trailblazor.Routing.App.WASM;

internal sealed class RoutingProfile : IRoutingProfile
{
    public void ConfigureRoutes(IRoutingConfigurationBuilder builder)
    {
        builder.AddNode<Home>("Home", "/");
        builder.AddNode("Content", g =>
        {
            g.AddNode<Counter>("Counter", "/counter");
            g.AddNode<Counter>("Counter", "/counter/{count}");
            g.AddNode<Weather>("Weather", "/weather");
        });
    }
}
