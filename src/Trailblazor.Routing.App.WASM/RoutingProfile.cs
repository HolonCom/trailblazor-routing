using Trailblazor.Routing.App.WASM.Pages;
using Trailblazor.Routing.Configuration;

namespace Trailblazor.Routing.App.WASM;

internal sealed class RoutingProfile : IRoutingProfile
{
    public void ConfigureRoutes(IRoutingConfigurationBuilder builder)
    {
        builder.AddNode<Home>("Home", "/", n => n.WithUris("/home", "/landing-page"));
        builder.AddNode("Content", g =>
        {
            g.AddNode<Counter>("Counter", "/counter", n => n.WithUris("/counter/{count}"));
            g.AddNode<Weather>("Weather", "/weather");
        });
    }
}
