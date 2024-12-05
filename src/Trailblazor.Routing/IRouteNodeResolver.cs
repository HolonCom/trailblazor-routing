namespace Trailblazor.Routing;

public interface IRouteNodeResolver
{
    public RouteResolveResult ResolveRouteForUri(string uri);
}
