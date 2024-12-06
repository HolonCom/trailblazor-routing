using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;
using Trailblazor.Routing.Configuration.Validation;
using Trailblazor.Routing.DependencyInjection;

namespace Trailblazor.Routing.Configuration;

internal sealed class RoutingConfigurationResolver(
    IRoutingOptionsProvider _routingOptionsProvider,
    IServiceProvider _serviceProvider,
    IRoutingConfigurationValidator _routingConfigurationValidator) : IRoutingConfigurationResolver
{
    public IRoutingConfiguration ResolveRoutingConfiguration()
    {
        try
        {
            var routingConfigurationBuilder = new RoutingConfigurationBuilder();
            var routingProfiles = _serviceProvider.GetServices<IRoutingProfile>().Concat([new InternalScanningProfile(_routingOptionsProvider)]);

            foreach (var profile in routingProfiles)
                profile.ConfigureRoutes(routingConfigurationBuilder);

            _routingOptionsProvider.GetRoutingOptions().ProfileAction?.Invoke(routingConfigurationBuilder);

            var routingConfiguration = routingConfigurationBuilder.Build();
            _routingConfigurationValidator.ValidateAndThrowIfInvalid(routingConfiguration);

            return routingConfiguration;
        }
        catch (Exception ex)
        {
            _serviceProvider.GetService<ILogger<RoutingConfigurationResolver>>()?.LogError(ex, ex.ToString());
            throw;
        }
    }
}
