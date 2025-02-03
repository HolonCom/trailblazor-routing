using System.Diagnostics.CodeAnalysis;

namespace Trailblazor.Routing.Routes;


/// <summary>
/// Standard route builder for building <see cref="Route"/>s.
/// </summary>
/// <typeparam name="TComponent">Type of component representing the route.</typeparam>
public class RouteBuilder
{
    private readonly Route _route;

    internal RouteBuilder(Type componentType)
    {
        _route = Route.Empty(componentType);
    }

    /// <summary>
    /// Constructor acceppts a <paramref name="baseRoute"/> the to be configured route bases off of.
    /// </summary>
    /// <param name="baseRoute">Base route the to be configured route bases off of.</param>
    internal RouteBuilder(Type componentType, Route baseRoute)
    {
        _route = baseRoute;
        _route.Component = componentType;
    }

    /// <summary>
    /// Method adds the specified <paramref name="value"/> for the <paramref name="key"/> to the routes metadata.
    /// </summary>
    /// <param name="key">Key of the metadata value.</param>
    /// <param name="value">Value for the <paramref name="key"/>.</param>
    /// <returns>Route builder for further configurations.</returns>
    public RouteBuilder WithMetadataValue(string key, object? value)
    {
        _route.SetMetadataValue(key, value);
        return this;
    }

    /// <summary>
    /// Method adds the specified <paramref name="metadata"/> to the route.
    /// </summary>
    /// <param name="metadata">Metadata to be added.</param>
    /// <returns>Route builder for further configurations.</returns>
    public RouteBuilder WithMetadata(Dictionary<string, object?> metadata)
    {
        _route.MergeMetadata(metadata);
        return this;
    }

    /// <summary>
    /// Method sets the specified <paramref name="uri"/> to be the routes URI.
    /// </summary>
    /// <param name="uri">The routes URI.</param>
    /// <returns>Route builder for further configurations.</returns>
    public RouteBuilder WithUri([StringSyntax(StringSyntaxAttribute.Uri)] string uri)
    {
        _route.Uri = uri.TrimStart('/');
        return this;
    }

    /// <summary>
    /// Method adds a child route to the route.
    /// </summary>
    /// <typeparam name="TChildComponent">Type of child route.</typeparam>
    /// <param name="builderAction">Builder action for the configuration of the child route.</param>
    /// <returns>Route builder for further configurations.</returns>
    public RouteBuilder WithChild(Type componentType, Action<RouteBuilder> builderAction)
    {
        var builder = new RouteBuilder(componentType).SetParent(_route);

        builderAction.Invoke(builder);
        _route.Children.Add(builder.Build());

        return this;
    }

    /// <summary>
    /// Method sets the specified <paramref name="parentRoute"/> to be the configured routes parent.
    /// </summary>
    /// <param name="parentRoute">The to be configured routes parent.</param>
    /// <returns>Route builder for further configurations.</returns>
    internal RouteBuilder SetParent(Route parentRoute)
    {
        _route.Parent = parentRoute;
        return this;
    }

    /// <summary>
    /// Method builds and returns the configured routes.
    /// </summary>
    /// <returns>Configured route.</returns>
    internal Route Build()
    {
        if (_route.Component == null)
            throw new NullReferenceException($"The '{nameof(_route.Component)}' property cannot be null.");

        return _route;
    }
}


///// <summary>
///// Standard route builder for building <see cref="Route"/>s.
///// </summary>
///// <typeparam name="TComponent">Type of component representing the route.</typeparam>
//public sealed class RouteBuilder<TComponent> : RouteBuilder where TComponent : IComponent
//{
//    private readonly Route _route = Route.Empty<TComponent>();

//    internal RouteBuilder() { }

//    /// <summary>
//    /// Constructor acceppts a <paramref name="baseRoute"/> the to be configured route bases off of.
//    /// </summary>
//    /// <param name="baseRoute">Base route the to be configured route bases off of.</param>
//    internal RouteBuilder(Route baseRoute) :base(typeof(TComponent), baseRoute)
//    {
//    }

//    /// <summary>
//    /// Method adds the specified <paramref name="value"/> for the <paramref name="key"/> to the routes metadata.
//    /// </summary>
//    /// <param name="key">Key of the metadata value.</param>
//    /// <param name="value">Value for the <paramref name="key"/>.</param>
//    /// <returns>Route builder for further configurations.</returns>
//    public RouteBuilder<TComponent> WithMetadataValue(string key, object? value)
//    {
//        base.WithMetadataValue(key, value);
//        return this;
//    }

//    /// <summary>
//    /// Method adds the specified <paramref name="metadata"/> to the route.
//    /// </summary>
//    /// <param name="metadata">Metadata to be added.</param>
//    /// <returns>Route builder for further configurations.</returns>
//    public RouteBuilder<TComponent> WithMetadata(Dictionary<string, object?> metadata)
//    {
//        base.WithMetadata(metadata);
//        return this;
//    }

//    /// <summary>
//    /// Method sets the specified <paramref name="uri"/> to be the routes URI.
//    /// </summary>
//    /// <param name="uri">The routes URI.</param>
//    /// <returns>Route builder for further configurations.</returns>
//    public RouteBuilder<TComponent> WithUri([StringSyntax(StringSyntaxAttribute.Uri)] string uri)
//    {
//        base.WithUri(uri);
//        return this;
//    }

//    /// <summary>
//    /// Method adds a child route to the route.
//    /// </summary>
//    /// <typeparam name="TChildComponent">Type of child route.</typeparam>
//    /// <param name="builderAction">Builder action for the configuration of the child route.</param>
//    /// <returns>Route builder for further configurations.</returns>
//    public RouteBuilder<TComponent> WithChild<TChildComponent>(Action<RouteBuilder<TChildComponent>> builderAction) where TChildComponent : IComponent
//    {
//        base.WithChild(typeof(TChildComponent, ))
//        return this;
//    }

//    /// <summary>
//    /// Method sets the specified <paramref name="parentRoute"/> to be the configured routes parent.
//    /// </summary>
//    /// <param name="parentRoute">The to be configured routes parent.</param>
//    /// <returns>Route builder for further configurations.</returns>
//    internal RouteBuilder<TComponent> SetParent(Route parentRoute)
//    {
//        base.SetP
//        return this;
//    }

//    /// <summary>
//    /// Method builds and returns the configured routes.
//    /// </summary>
//    /// <returns>Configured route.</returns>
//    internal Route Build()
//    {
//        if (_route.Component == null)
//            throw new NullReferenceException($"The '{nameof(_route.Component)}' property cannot be null.");

//        return _route;
//    }
//}
