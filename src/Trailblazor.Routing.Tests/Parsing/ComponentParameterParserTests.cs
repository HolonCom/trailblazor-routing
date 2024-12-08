using Microsoft.Extensions.DependencyInjection;
using Trailblazor.Routing.Exceptions;
using Trailblazor.Routing.Parsing;
using Trailblazor.Routing.Tests.DI;
using Trailblazor.Routing.Tests.Mocks;

namespace Trailblazor.Routing.Tests.Parsing;

public class ComponentParameterParserTests
{
    private readonly IServiceProvider _serviceProvider = TestServiceProviderFactory.Create();
    private IComponentParameterParser ComponentParameterParser => _serviceProvider.GetRequiredService<IComponentParameterParser>();

    [Fact]
    public void GetComponentParametersFromQueryParameters_InvalidPropertyWithoutParameterAttribute_IsBeingIgnored()
    {
        var queryParameters = new Dictionary<string, string>()
        {
            { "missingParameterAttribute", "0" },
        };

        Assert.Throws<RoutingParameterException>(() => ComponentParameterParser.GetComponentParametersFromQueryParameters(typeof(ComponentMock), queryParameters));
    }

    [Fact]
    public void GetComponentParametersFromQueryParameters_ParsesString()
    {
        var queryParameters = new Dictionary<string, string>()
        {
            { "customStringParameter", "test" },
        };

        var componentParameters = ComponentParameterParser.GetComponentParametersFromQueryParameters(typeof(ComponentMock), queryParameters);
        Assert.Equal("test", componentParameters["StringParameter"]);
    }

    [Fact]
    public void GetComponentParametersFromQueryParameters_ParsesBool()
    {
        var queryParameters = new Dictionary<string, string>()
        {
            { "customBoolParameter", "true" },
        };

        var componentParameters = ComponentParameterParser.GetComponentParametersFromQueryParameters(typeof(ComponentMock), queryParameters);
        Assert.Equal(true, componentParameters["BoolParameter"]);
    }

    [Fact]
    public void GetComponentParametersFromQueryParameters_ParsesGuid()
    {
        var guid = Guid.NewGuid();
        var queryParameters = new Dictionary<string, string>()
        {
            { "customGuidParameter", guid.ToString() },
        };

        var componentParameters = ComponentParameterParser.GetComponentParametersFromQueryParameters(typeof(ComponentMock), queryParameters);
        Assert.Equal(guid, componentParameters["GuidParameter"]);
    }

    [Fact]
    public void GetComponentParametersFromQueryParameters_ParsesTimeOnly()
    {
        var time = new TimeOnly(14, 30);
        var queryParameters = new Dictionary<string, string>()
        {
            { "customTimeOnlyParameter", time.ToString("HH:mm:ss.fff") },
        };

        var componentParameters = ComponentParameterParser.GetComponentParametersFromQueryParameters(typeof(ComponentMock), queryParameters);
        Assert.Equal(time, componentParameters["TimeOnlyParameter"]);
    }

    [Fact]
    public void GetComponentParametersFromQueryParameters_ParsesDateOnly()
    {
        var date = new DateOnly(2023, 11, 5);
        var queryParameters = new Dictionary<string, string>()
        {
            { "customDateOnlyParameter", date.ToString("yyyy-MM-dd") },
        };

        var componentParameters = ComponentParameterParser.GetComponentParametersFromQueryParameters(typeof(ComponentMock), queryParameters);
        Assert.Equal(date, componentParameters["DateOnlyParameter"]);
    }

    [Fact]
    public void GetComponentParametersFromQueryParameters_ParsesDateTime()
    {
        var dateTime = new DateTime(2023, 11, 5, 14, 30, 0, DateTimeKind.Utc);
        var queryParameters = new Dictionary<string, string>()
        {
            { "customDateTimeParameter", dateTime.ToString("o") },
        };

        var componentParameters = ComponentParameterParser.GetComponentParametersFromQueryParameters(typeof(ComponentMock), queryParameters);
        Assert.Equal(dateTime, componentParameters["DateTimeParameter"]);
    }

    [Fact]
    public void GetComponentParametersFromQueryParameters_ParsesInt()
    {
        var queryParameters = new Dictionary<string, string>()
        {
            { "customIntParameter", "123" },
        };

        var componentParameters = ComponentParameterParser.GetComponentParametersFromQueryParameters(typeof(ComponentMock), queryParameters);
        Assert.Equal(123, componentParameters["IntParameter"]);
    }

    [Fact]
    public void GetComponentParametersFromQueryParameters_ParsesDouble()
    {
        var queryParameters = new Dictionary<string, string>()
        {
            { "customDoubleParameter", "123.45" },
        };

        var componentParameters = ComponentParameterParser.GetComponentParametersFromQueryParameters(typeof(ComponentMock), queryParameters);
        Assert.Equal(123.45, componentParameters["DoubleParameter"]);
    }

    [Fact]
    public void GetComponentParametersFromQueryParameters_ParsesLong()
    {
        var queryParameters = new Dictionary<string, string>()
        {
            { "customLongParameter", "1234567890" },
        };

        var componentParameters = ComponentParameterParser.GetComponentParametersFromQueryParameters(typeof(ComponentMock), queryParameters);
        Assert.Equal(1234567890L, componentParameters["LongParameter"]);
    }

    [Fact]
    public void GetComponentParametersFromQueryParameters_ParsesDecimal()
    {
        var queryParameters = new Dictionary<string, string>()
        {
            { "customDecimalParameter", "12345.6789" },
        };

        var componentParameters = ComponentParameterParser.GetComponentParametersFromQueryParameters(typeof(ComponentMock), queryParameters);
        Assert.Equal(12345.6789m, componentParameters["DecimalParameter"]);
    }
}
