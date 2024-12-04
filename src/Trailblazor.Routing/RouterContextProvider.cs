using Microsoft.AspNetCore.Components;
using Trailblazor.Routing.Extensions;
using Trailblazor.Routing.Parsing;

namespace Trailblazor.Routing;

internal sealed class RouterContextProvider(
    NavigationManager _navigationManager,
    INodeProvider _routeProvider,
    IComponentParameterParser _componentParameterParser,
    IUriParser _uriParser) : IRouterContextProvider
{
    private RouterContext? _routerContext;

    public RouterContext GetRouterContext()
    {
        var currentUri = _navigationManager.GetRelativeUri();
        var currentUriWithoutQueryParameters = _uriParser.GetUriWithoutQueryParameters(currentUri);
        var queryParameters = _uriParser.GetQueryParametersFromUri(currentUri);

        return _routerContext ??= CreateRouterContextFromUri(
            currentUriWithoutQueryParameters,
            queryParameters);
    }

    public void UpdateRouterContext()
    {
        var relativeUri = _navigationManager.GetRelativeUri();
        var uriWithoutQueryParameters = _uriParser.GetUriWithoutQueryParameters(relativeUri);
        var queryParameters = _uriParser.GetQueryParametersFromUri(relativeUri);

        _routerContext = CreateRouterContextFromUri(
            uriWithoutQueryParameters,
            queryParameters);
    }

    private RouterContext CreateRouterContextFromUri(string uri, Dictionary<string, string> queryParameters)
    {
        var currentRoute = _routeProvider.FindRoute(uri);
        RouteData routeData;
        Dictionary<string, object> componentQueryParameters = [];

        if (currentRoute == null)
        {
            // TODO -> Use a NotFoundComponent instead!
            routeData = new RouteData(typeof(ComponentBase), new Dictionary<string, object?>());
        }
        else
        {
            componentQueryParameters = _componentParameterParser.GetComponentParametersFromQueryParameters(currentRoute.ComponentType, queryParameters!);
            routeData = new RouteData(currentRoute.ComponentType, componentQueryParameters!);
        }

        return new RouterContext()
        {
            Route = currentRoute,
            RouteData = routeData,
            QueryParameters = componentQueryParameters,
        };
    }
}
