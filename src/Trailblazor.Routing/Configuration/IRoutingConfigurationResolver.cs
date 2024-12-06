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
    internal IRoutingConfiguration ResolveRoutingConfiguration();
}
