using Microsoft.AspNetCore.Components;
using Trailblazor.Routing.Components;
using Trailblazor.Routing.Configuration;
using Trailblazor.Routing.Exceptions;
using Trailblazor.Routing.Extensions;

namespace Trailblazor.Routing;

internal sealed class RouterContextProvider(
    NavigationManager _navigationManager,
    IRoutingConfigurationProvider _routingConfigurationProvider,
    IRouteNodeResolver _routeNodeResolver) : IRouterContextProvider
{
    private RouterContext? _routerContext;

    public RouterContext GetRouterContext()
    {
        return _routerContext ??= CreateRouterContextFromUri();
    }

    public void UpdateRouterContext()
    {
        _routerContext = CreateRouterContextFromUri();
    }

    private RouterContext CreateRouterContextFromUri()
    {
        var routeResult = _routeNodeResolver.ResolveRouteForUri(_navigationManager.GetRelativeUri());
        RouteData routeData;

        if (routeResult.RouteNode == null)
        {
            var notFoundRedirectUri = _routingConfigurationProvider.GetRoutingConfiguration().NotFoundRedirectUri;
            if (notFoundRedirectUri != null)
                _navigationManager.NavigateTo(notFoundRedirectUri);

            var notFoundComponentType = _routingConfigurationProvider.GetRoutingConfiguration().NotFoundComponentType;
            routeData = new RouteData(notFoundComponentType ?? typeof(NotFound), new Dictionary<string, object?>());
        }
        else
        {
            if (routeResult.RouteNode.ComponentType == null)
                throw new RoutingValidationException($"Route with the key '{routeResult.RouteNode.Key}' was resolved but does not have a configured component type and thus cannot be rendered.");

            routeData = new RouteData(routeResult.RouteNode.ComponentType, routeResult.ComponentParameters!);
        }

        return new RouterContext()
        {
            RouteNode = routeResult.RouteNode,
            RouteParameters = routeResult.ComponentParameters,
            RouteData = routeData,
        };
    }
}
