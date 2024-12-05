namespace Trailblazor.Routing.Parsing;

/// <summary>
/// Service parses URIs and their query parameters.
/// </summary>
public interface IUriParser
{
    /// <summary>
    /// Method gets the <paramref name="uri"/> without its query parameters.
    /// </summary>
    /// <param name="uri">URI with query parameters.</param>
    /// <returns>URI without query parameters.</returns>
    public string GetUriWithoutQueryParameters(string uri);

    /// <summary>
    /// Method combines the provided <paramref name="uri"/> and <paramref name="queryParameters"/>.
    /// </summary>
    /// <remarks>
    /// The <paramref name="queryParameters"/> will be parsed to ISO 8601 strings for query parameter combatibility.
    /// </remarks>
    /// <param name="uri">URI to be decorated with the <paramref name="queryParameters"/>.</param>
    /// <param name="queryParameters">Query parameters to be added to the <paramref name="uri"/>.</param>
    /// <returns><paramref name="uri"/> with <paramref name="queryParameters"/> added to it.</returns>
    public string GetUriWithQueryParameters(string uri, Dictionary<string, object?> queryParameters);

    /// <summary>
    /// Method combines the provided <paramref name="uri"/> and <paramref name="queryParameters"/>.
    /// </summary>
    /// <remarks>
    /// The <paramref name="queryParameters"/> need to be parsed to ISO 8601 strings for query parameter combatibility.
    /// If they are not, they might not be able to be parsed from a string into their respective underlying type.
    /// </remarks>
    /// <param name="uri">URI to be decorated with the <paramref name="queryParameters"/>.</param>
    /// <param name="queryParameters">Query parameters to be added to the <paramref name="uri"/>.</param>
    /// <returns><paramref name="uri"/> with <paramref name="queryParameters"/> added to it.</returns>
    public string GetUriWithQueryParameters(string uri, Dictionary<string, string?> queryParameters);

    /// <summary>
    /// Method gets the query parameters of the provided <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">URI whose query parameters are to be extracted.</param>
    /// <returns>Query parameters of the provided <paramref name="uri"/>.</returns>
    public Dictionary<string, string> GetQueryParametersFromUri(string uri);
}
