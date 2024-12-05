using System.Globalization;
using System.Reflection;
using Trailblazor.Routing.Navigation;
using Trailblazor.Routing.Parsing.Exceptions;
using Trailblazor.Routing.Parsing.Extensions;

namespace Trailblazor.Routing.Parsing;

internal sealed class ComponentParameterParser : IComponentParameterParser
{
    Dictionary<string, object> IComponentParameterParser.GetComponentParametersFromQueryParameters(Type componentType, Dictionary<string, string> unparserComponentParameters)
    {
        var properties = componentType.GetProperties();
        var parsedParameters = new Dictionary<string, object>();

        foreach (var unparsedParameter in unparserComponentParameters)
        {
            QueryParameterAttribute? queryParameterAttribute = null;
            var targetProperty = properties.SingleOrDefault(p =>
            {
                var propertyNameMatches = p.Name == unparsedParameter.Key;
                if (propertyNameMatches)
                    return true;

                queryParameterAttribute = p.GetCustomAttribute<QueryParameterAttribute>();
                if (queryParameterAttribute == null)
                    return false;

                return string.Equals(queryParameterAttribute.Name, unparsedParameter.Key, StringComparison.InvariantCultureIgnoreCase);
            });

            if (targetProperty == null)
                continue;

            RoutingParameterException.ThrowIfPropertyDoesntHaveParameterAttribute(targetProperty, componentType.Name);

            var queryParameterValue = ParseValueForProperty(targetProperty.PropertyType, unparsedParameter.Value);
            if (queryParameterValue != null)
                parsedParameters[targetProperty.Name] = queryParameterValue;
        }

        return parsedParameters;
    }

    private object? ParseValueForProperty(Type propertyType, string? parameterValueString)
    {
        try
        {
            if (parameterValueString == null)
                return null;

            parameterValueString = Uri.UnescapeDataString(parameterValueString);

            if (propertyType == typeof(string) || propertyType == typeof(object))
                return parameterValueString;
            else if (propertyType == typeof(bool) && bool.TryParse(parameterValueString, out var boolValue))
                return boolValue;
            else if (propertyType.IsGuid() && Guid.TryParse(parameterValueString, out var guidValue))
                return guidValue;
            else if (propertyType.IsTimeOnly() && TimeOnly.TryParse(parameterValueString, out var timeOnlyValue))
                return timeOnlyValue;
            else if (propertyType.IsDateOnly() && DateOnly.TryParse(parameterValueString, out var dateOnlyValue))
                return dateOnlyValue;
            else if (propertyType.IsDateTime() && DateTime.TryParse(parameterValueString, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var dateTimeValue))
                return dateTimeValue;
            else if (propertyType.IsInt() && int.TryParse(parameterValueString, CultureInfo.InvariantCulture, out var intValue))
                return intValue;
            else if (propertyType.IsDouble() && double.TryParse(parameterValueString, CultureInfo.InvariantCulture, out var doubleValue))
                return doubleValue;
            else if (propertyType.IsLong() && long.TryParse(parameterValueString, CultureInfo.InvariantCulture, out var longValue))
                return longValue;
            else if (propertyType.IsDecimal() && decimal.TryParse(parameterValueString, CultureInfo.InvariantCulture, out var decimalValue))
                return decimalValue;
        }
        catch
        {
        }

        return null;
    }
}
