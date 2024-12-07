using Trailblazor.Routing.Exceptions;

namespace Trailblazor.Routing.Configuration;

/// <summary>
/// Service provides the configured <see cref="IRoutingConfiguration"/>.
/// </summary>
public interface IRoutingConfigurationProvider
{
    /// <summary>
    /// Method gets the configured <see cref="IRoutingConfiguration"/>.
    /// </summary>
    /// <returns>The configured <see cref="IRoutingConfiguration"/>.</returns>
    /// <exception cref="RoutingValidationException">Thrown if any validation related exceptions occurr.</exception>
    public IRoutingConfiguration GetRoutingConfiguration();
}
