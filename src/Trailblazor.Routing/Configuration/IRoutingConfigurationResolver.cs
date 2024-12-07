using Trailblazor.Routing.Exceptions;

namespace Trailblazor.Routing.Configuration;

/// <summary>
/// Service internally resolves the routing <see cref="IRoutingConfiguration"/>.
/// </summary>
internal interface IRoutingConfigurationResolver
{
    /// <summary>
    /// Method resovles the <see cref="IRoutingConfiguration"/>.
    /// </summary>
    /// <returns>The resolved <see cref="IRoutingConfiguration"/>.</returns>
    /// <exception cref="RoutingValidationException">Thrown if any validation related exceptions occurr.</exception>
    internal IRoutingConfiguration ResolveRoutingConfiguration();
}
