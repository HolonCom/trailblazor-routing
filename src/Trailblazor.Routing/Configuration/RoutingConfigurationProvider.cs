namespace Trailblazor.Routing.Configuration;

internal sealed class RoutingConfigurationProvider(IRoutingConfigurationResolver _routingConfigurationResolver) : IRoutingConfigurationProvider
{
    private IRoutingConfiguration? _routingConfiguration;

    public IRoutingConfiguration GetRoutingConfiguration()
    {
        return _routingConfiguration ??= _routingConfigurationResolver.ResolveRoutingConfiguration();
    }
}
