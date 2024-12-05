using System.Globalization;

namespace Trailblazor.Routing.Parsing;

internal sealed class UriParser : IUriParser
{
    public string GetUriWithoutQueryParameters(string uri)
    {
        return uri.Contains('?') ? uri.Substring(0, uri.IndexOf('?')) : uri;
    }

    public string GetUriWithQueryParameters(string uri, Dictionary<string, object?> queryParameters)
    {
        var parsedQueryParameters = queryParameters.ToDictionary(k => k.Key, v => ConvertToIso8601(v.Value));
        return GetUriWithQueryParameters(uri, parsedQueryParameters);
    }

    public string GetUriWithQueryParameters(string uri, Dictionary<string, string?> queryParameters)
    {
        var validQueryParameters = queryParameters.Where(g => g.Key != null && g.Value != null)!.ToDictionary<string, string>();
        if (validQueryParameters.Count == 0)
            return uri;

        var uriWithParams = uri;
        if (uri.Contains('?'))
            uriWithParams += "&";   // Existing query parameters, append with '&'
        else
            uriWithParams += "?";   // No existing parameters, start with '?'

        uriWithParams += string.Join("&", validQueryParameters.Select(param =>
            $"{Uri.EscapeDataString(param.Key)}={Uri.EscapeDataString(param.Value)}"
        ));

        return uriWithParams;
    }

    public Dictionary<string, string> GetQueryParametersFromUri(string uri)
    {
        var queryParameters = new Dictionary<string, string>();

        if (!uri.Contains('?'))
            return queryParameters;

        //var queryString = uri.Substring(uri.IndexOf('?') + 1);
        var queryString = uri.Split('?')[1];
        var queryParameterPairs = queryString.Split('&', StringSplitOptions.RemoveEmptyEntries);

        foreach (var queryParameterPair in queryParameterPairs)
        {
            var pair = queryParameterPair.Split('=');
            if (pair.Length == 2)
            {
                var decodedKey = Uri.UnescapeDataString(pair[0]);
                var decodedValue = Uri.UnescapeDataString(pair[1]);
                queryParameters[decodedKey] = decodedValue;
            }
            else if (pair.Length == 1)
            {
                var decodedKey = Uri.UnescapeDataString(pair[0]);
                queryParameters[decodedKey] = string.Empty;
            }
        }

        return queryParameters;
    }

    private static string? ConvertToIso8601(object? value)
    {
        if (value == null)
            return null;

        var stringValue = value switch
        {
            DateTime dateTime => dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"),
            DateOnly dateOnly => dateOnly.ToString("yyyy-MM-dd"),
            TimeOnly timeOnly => timeOnly.ToString("HH:mm:ss"),
            int @int => @int.ToString(CultureInfo.InvariantCulture),
            long @long => @long.ToString(CultureInfo.InvariantCulture),
            decimal @decimal => @decimal.ToString(CultureInfo.InvariantCulture),
            double @double => @double.ToString(CultureInfo.InvariantCulture),
            _ => value.ToString(),
        };

        return stringValue;
    }
}