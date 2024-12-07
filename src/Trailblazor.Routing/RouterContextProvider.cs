using Microsoft.AspNetCore.Components;
using Trailblazor.Routing.Components;
using Trailblazor.Routing.Configuration;
using Trailblazor.Routing.Exceptions;
using Trailblazor.Routing.Extensions;

namespace Trailblazor.Routing;

internal sealed class RouterContextProvider(
    NavigationManager _navigationManager,
    IRoutingConfigurationProvider _routingConfigurationProvider,
    INodeResolver _routeNodeResolver) : IRouterContextProvider
{
    private RouterContext? _routerContext;
    private event EventHandler<RouterContextUpdatedEventArgs>? OnRouterContextChangedEvent;

    public RouterContext GetRouterContext()
    {
        return _routerContext ??= CreateRouterContextFromUri();
    }

    public void Subscribe(EventHandler<RouterContextUpdatedEventArgs> eventHandler)
    {
        OnRouterContextChangedEvent += eventHandler;
    }

    public void Unsubscribe(EventHandler<RouterContextUpdatedEventArgs> eventHandler)
    {
        OnRouterContextChangedEvent -= eventHandler;
    }

    public void UpdateRouterContext()
    {
        _routerContext = CreateRouterContextFromUri();
        OnRouterContextChangedEvent?.Invoke(this, new() { Context = _routerContext, });
    }

    private RouterContext CreateRouterContextFromUri()
    {
        var routeResult = _routeNodeResolver.ResolveNodeForUri(_navigationManager.GetRelativeUri());
        RouteData routeData;

        if (routeResult.Node == null)
        {
            var notFoundRedirectUri = _routingConfigurationProvider.GetRoutingConfiguration().NotFoundRedirectUri;
            if (notFoundRedirectUri != null)
                _navigationManager.NavigateTo(notFoundRedirectUri);

            var notFoundComponentType = _routingConfigurationProvider.GetRoutingConfiguration().NotFoundComponentType;
            routeData = new RouteData(notFoundComponentType ?? typeof(NotFound), new Dictionary<string, object?>());
        }
        else
        {
            if (routeResult.Node.ComponentType == null)
                throw new RoutingValidationException($"Route with the key '{routeResult.Node.Key}' was resolved but does not have a configured component type and thus cannot be rendered.");

            routeData = new RouteData(routeResult.Node.ComponentType, routeResult.ComponentParameters!);
        }

        return new RouterContext()
        {
            Node = routeResult.Node,
            ComponentParameters = routeResult.ComponentParameters,
            RouteData = routeData,
        };
    }
}
