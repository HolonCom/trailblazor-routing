namespace Trailblazor.Routing;

/// <summary>
/// Event arguments for when the current <see cref="RouterContext"/> has been updated.
/// </summary>
public sealed class RouterContextUpdatedEventArgs : EventArgs
{
    internal RouterContextUpdatedEventArgs() { }

    /// <summary>
    /// Changed router context.
    /// </summary>
    public required RouterContext Context { get; init; }
}
