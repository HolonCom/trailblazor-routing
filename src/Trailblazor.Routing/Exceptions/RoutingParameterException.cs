using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Trailblazor.Routing.Exceptions;

public sealed class RoutingParameterException(string message) : Exception(message)
{
    public static void ThrowIfPropertyDoesntHaveParameterAttribute(PropertyInfo propertyInfo, string componentName)
    {
        var parameterAttribute = propertyInfo.GetCustomAttribute<ParameterAttribute>();
        if (parameterAttribute != null)
            return;

        throw new RoutingParameterException($"No attribute of type '{nameof(ParameterAttribute)}' has been found on property '{propertyInfo.Name}' of component '{componentName}'. However this attribute is mandatory for the values of parameter properties to be set.");
    }
}
