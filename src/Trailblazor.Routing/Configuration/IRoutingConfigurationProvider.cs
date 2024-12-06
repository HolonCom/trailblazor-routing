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
    public IRoutingConfiguration GetRoutingConfiguration();
}
