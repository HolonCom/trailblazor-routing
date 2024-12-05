using Microsoft.AspNetCore.Components;
using Trailblazor.Routing.Extensions;

namespace Trailblazor.Routing;

internal sealed class RouterContextProvider(
    NavigationManager _navigationManager,
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
            // TODO -> Use a NotFoundComponent instead!
            routeData = new RouteData(typeof(ComponentBase), new Dictionary<string, object?>());
        }
        else
        {
            routeData = new RouteData(routeResult.RouteNode.ComponentType, routeResult.ComponentParameters!);
        }

        return new RouterContext()
        {
            Route = routeResult.RouteNode,
            RouteParameters = routeResult.ComponentParameters,
            RouteData = routeData,
        };
    }
}
