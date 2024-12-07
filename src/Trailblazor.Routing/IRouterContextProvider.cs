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
    /// Subscribes to the event that is being invoked when the router context gets updated.
    /// </summary>
    /// <param name="eventHandler">Hander of the event.</param>
    public void Subscribe(EventHandler<RouterContextUpdatedEventArgs> eventHandler);

    /// <summary>
    /// Unsubscribes from the event that is being invoked when the router context gets updated.
    /// </summary>
    /// <param name="eventHandler">Hander of the event.</param>
    public void Unsubscribe(EventHandler<RouterContextUpdatedEventArgs> eventHandler);

    /// <summary>
    /// Method internally prompts the provider to update itself and create a new context.
    /// </summary>
    internal void UpdateRouterContext();
}
