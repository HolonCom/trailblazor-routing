using System.Text.RegularExpressions;
using Trailblazor.Routing.Configuration;
using Trailblazor.Routing.Exceptions;
using Trailblazor.Routing.Parsing;

namespace Trailblazor.Routing;

internal sealed class RouteNodeResolver(
    IRoutingConfigurationProvider _routingConfigurationProvider,
    IComponentParameterParser _componentParameterParser,
    IUriParser _uriParser) : IRouteNodeResolver
{
    public RouteResolveResult ResolveRouteForUri(string currentUri)
    {
        currentUri = currentUri.Trim('/');
        var currentUriWithoutQueryParameters = _uriParser.GetUriWithoutQueryParameters(currentUri);

        foreach (var routeNode in _routingConfigurationProvider.GetRoutingConfiguration().FlattenedNodes)
        {
            Dictionary<string, string> currentUriParameters = [];
            var routeUriFound = false;

            foreach (var routeUri in routeNode.Uris)
            {
                currentUriParameters = GetUriParametersFromTheCurrentUriUsingTheRouteUriPattern(currentUriWithoutQueryParameters, routeUri);
                if (routeUri == currentUriWithoutQueryParameters || currentUriParameters.Count != 0)
                {
                    routeUriFound = true;
                    break;
                }
            }

            if (!routeUriFound)
                continue;

            if (routeNode.ComponentType == null)
                throw new RoutingValidationException($"The node with the key '{routeNode.Key}' matches the current relative URI but has no associated component type.");

            var currentUriQueryParameters = _uriParser.GetQueryParametersFromUri(currentUri);
            return new RouteResolveResult()
            {
                RouteNode = routeNode,
                ComponentParameters = _componentParameterParser.GetComponentParametersFromQueryParameters(
                    routeNode.ComponentType,
                    currentUriParameters.Concat(currentUriQueryParameters).ToDictionary()),
            };
        }

        return RouteResolveResult.Empty;
    }

    private Dictionary<string, string> GetUriParametersFromTheCurrentUriUsingTheRouteUriPattern(string currentUri, string routeUri)
    {
        var routeUriRegex = CreateUriParameterRegexFromUri(routeUri);
        var match = routeUriRegex.Match(currentUri);
        if (!match.Success)
            return [];

        var parameters = new Dictionary<string, string>();
        foreach (var groupName in routeUriRegex.GetGroupNames())
        {
            if (int.TryParse(groupName, out _))
                continue;

            parameters[groupName] = match.Groups[groupName].Value;
        }

        return parameters;
    }

    private Regex CreateUriParameterRegexFromUri(string uri)
    {
        var pattern = Regex.Replace(uri, @"\{(\w+):(\w+)\}", m =>
        {
            var paramName = m.Groups[1].Value;
            var paramType = m.Groups[2].Value;

            return paramType switch
            {
                "int" => $"(?<{paramName}>-?\\d+)",
                "long" => $"(?<{paramName}>-?\\d+)",
                "guid" => $"(?<{paramName}>[0-9a-fA-F-{{}}]+)",
                "string" => $"(?<{paramName}>[^/]+)",
                "bool" => $"(?<{paramName}>true|false)",
                "datetime" => $"(?<{paramName}>\\d{{4}}-\\d{{2}}-\\d{{2}}(?:\\s[\\d:apmAPM ]+)?)",
                "decimal" => $"(?<{paramName}>-?\\d+(\\.\\d+)?)",
                "double" => $"(?<{paramName}>-?\\d+(\\.\\d+)?(e[+-]?\\d+)?)",
                "float" => $"(?<{paramName}>-?\\d+(\\.\\d+)?(e[+-]?\\d+)?)",
                "nonfile" => $"(?<{paramName}>[^/]+(?<!\\.css|\\.ico))",
                _ => throw new NotSupportedException($"Unsupported route parameter type: {paramType}")
            };
        });

        pattern = Regex.Replace(pattern, @"\{(\w+)\}", m =>
        {
            var paramName = m.Groups[1].Value;
            return $"(?<{paramName}>[^/]+)";
        });

        return new Regex($"^{pattern}$");
    }
}
