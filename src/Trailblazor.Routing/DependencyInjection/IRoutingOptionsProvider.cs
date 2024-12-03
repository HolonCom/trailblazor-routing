namespace Trailblazor.Routing.DependencyInjection;

/// <summary>
/// Service provides the configured <see cref="IRoutingOptions"/>.
/// </summary>
public interface IRoutingOptionsProvider
{
    /// <summary>
    /// Gets the configured <see cref="IRoutingOptions"/>.
    /// </summary>
    /// <returns>The configured <see cref="IRoutingOptions"/>.</returns>
    public IRoutingOptions GetRoutingOptions();
}
