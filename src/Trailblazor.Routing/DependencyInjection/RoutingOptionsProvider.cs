namespace Trailblazor.Routing.DependencyInjection;

internal sealed class RoutingOptionsProvider(IRoutingOptions routingOptions) : IRoutingOptionsProvider
{
    private readonly IRoutingOptions _routingOptions = routingOptions;

    public IRoutingOptions GetRoutingOptions()
    {
        return _routingOptions;
    }
}
