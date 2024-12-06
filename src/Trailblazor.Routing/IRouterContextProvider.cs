namespace Trailblazor.Routing;

/// <summary>
/// Service provides the current <see cref="RouterContext"/>.
/// </summary>
public interface IRouterContextProvider
{
    /// <summary>
    /// Method gets the current <see cref="RouterContext"/>.
    /// </summary>
    /// <returns>The current <see cref="RouterContext"/>.</returns>
    public RouterContext GetRouterContext();

    /// <summary>
    /// Method internally prompts the provider to update itself and create a new context.
    /// </summary>
    internal void UpdateRouterContext();
}
