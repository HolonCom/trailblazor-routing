using Microsoft.Extensions.DependencyInjection;
using Trailblazor.Routing.Parsing;
using Trailblazor.Routing.Tests.DI;
using Trailblazor.Routing.Tests.Mocks;

namespace Trailblazor.Routing.Tests.Parsing;

public class CombinedParsingTests
{
    private readonly IServiceProvider _serviceProvider = TestServiceProviderFactory.Create();

    private IUriParser UriParser => _serviceProvider.GetRequiredService<IUriParser>();
    private IComponentParameterParser ComponentParameterParser => _serviceProvider.GetRequiredService<IComponentParameterParser>();

    [Fact]
    public void CombinedParsing_DateTime()
    {
        var uri = "https://example.com/path";
        var dateTimeValue = new DateTime(2023, 11, 5, 13, 45, 30).ToUniversalTime();
        var queryParameters = new Dictionary<string, object?>()
        {
            { "customDateTimeParameter", dateTimeValue }
        };

        var uriWithQueryParameters = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        var rawQueryParameters = UriParser.GetQueryParametersFromUri(uriWithQueryParameters);

        var componentParameters = ComponentParameterParser.GetComponentParametersFromQueryParameters(typeof(ComponentMock), rawQueryParameters!);
        Assert.Equal(dateTimeValue, componentParameters[nameof(ComponentMock.DateTimeParameter)]);
    }

    [Fact]
    public void CombinedParsing_TimeOnly()
    {
        var uri = "https://example.com/path";
        var timeOnlyValue = new TimeOnly(13, 45, 30);
        var queryParameters = new Dictionary<string, object?>()
        {
            { "customTimeOnlyParameter", timeOnlyValue }
        };

        var uriWithQueryParameters = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        var rawQueryParameters = UriParser.GetQueryParametersFromUri(uriWithQueryParameters);

        var componentParameters = ComponentParameterParser.GetComponentParametersFromQueryParameters(typeof(ComponentMock), rawQueryParameters!);
        Assert.Equal(timeOnlyValue, componentParameters[nameof(ComponentMock.TimeOnlyParameter)]);
    }

    [Fact]
    public void CombinedParsing_DateOnly()
    {
        var uri = "https://example.com/path";
        var dateOnlyValue = new DateOnly(2023, 11, 5);
        var queryParameters = new Dictionary<string, object?>()
        {
            { "customDateOnlyParameter", dateOnlyValue }
        };

        var uriWithQueryParameters = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        var rawQueryParameters = UriParser.GetQueryParametersFromUri(uriWithQueryParameters);

        var componentParameters = ComponentParameterParser.GetComponentParametersFromQueryParameters(typeof(ComponentMock), rawQueryParameters!);
        Assert.Equal(dateOnlyValue, componentParameters[nameof(ComponentMock.DateOnlyParameter)]);
    }

    [Fact]
    public void CombinedParsing_Double()
    {
        var uri = "https://example.com/path";
        var doubleValue = 12.3;
        var queryParameters = new Dictionary<string, object?>()
        {
            { "customDoubleParameter", doubleValue }
        };

        var uriWithQueryParameters = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        var rawQueryParameters = UriParser.GetQueryParametersFromUri(uriWithQueryParameters);

        var componentParameters = ComponentParameterParser.GetComponentParametersFromQueryParameters(typeof(ComponentMock), rawQueryParameters!);
        Assert.Equal(doubleValue, componentParameters[nameof(ComponentMock.DoubleParameter)]);
    }

    [Fact]
    public void CombinedParsing_Decimal()
    {
        var uri = "https://example.com/path";
        var decimalValue = 12.3m;
        var queryParameters = new Dictionary<string, object?>()
        {
            { "customDecimalParameter", decimalValue }
        };

        var uriWithQueryParameters = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        var rawQueryParameters = UriParser.GetQueryParametersFromUri(uriWithQueryParameters);

        var componentParameters = ComponentParameterParser.GetComponentParametersFromQueryParameters(typeof(ComponentMock), rawQueryParameters!);
        Assert.Equal(decimalValue, componentParameters[nameof(ComponentMock.DecimalParameter)]);
    }

    [Fact]
    public void CombinedParsing_Int()
    {
        var uri = "https://example.com/path";
        var decimalValue = 12;
        var queryParameters = new Dictionary<string, object?>()
        {
            { "customIntParameter", decimalValue }
        };

        var uriWithQueryParameters = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        var rawQueryParameters = UriParser.GetQueryParametersFromUri(uriWithQueryParameters);

        var componentParameters = ComponentParameterParser.GetComponentParametersFromQueryParameters(typeof(ComponentMock), rawQueryParameters!);
        Assert.Equal(decimalValue, componentParameters[nameof(ComponentMock.IntParameter)]);
    }

    [Fact]
    public void CombinedParsing_Long()
    {
        var uri = "https://example.com/path";
        var longValue = 12L;
        var queryParameters = new Dictionary<string, object?>()
        {
            { "customLongParameter", longValue }
        };

        var uriWithQueryParameters = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        var rawQueryParameters = UriParser.GetQueryParametersFromUri(uriWithQueryParameters);

        var componentParameters = ComponentParameterParser.GetComponentParametersFromQueryParameters(typeof(ComponentMock), rawQueryParameters!);
        Assert.Equal(longValue, componentParameters[nameof(ComponentMock.LongParameter)]);
    }

    [Fact]
    public void CombinedParsing_Guid()
    {
        var uri = "https://example.com/path";
        var guidValue = Guid.NewGuid();
        var queryParameters = new Dictionary<string, object?>()
        {
            { "customGuidParameter", guidValue }
        };

        var uriWithQueryParameters = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        var rawQueryParameters = UriParser.GetQueryParametersFromUri(uriWithQueryParameters);

        var componentParameters = ComponentParameterParser.GetComponentParametersFromQueryParameters(typeof(ComponentMock), rawQueryParameters!);
        Assert.Equal(guidValue, componentParameters[nameof(ComponentMock.GuidParameter)]);
    }

    [Fact]
    public void CombinedParsing_Bool()
    {
        var uri = "https://example.com/path";
        var boolValue = true;
        var queryParameters = new Dictionary<string, object?>()
        {
            { "customBoolParameter", boolValue }
        };

        var uriWithQueryParameters = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        var rawQueryParameters = UriParser.GetQueryParametersFromUri(uriWithQueryParameters);

        var componentParameters = ComponentParameterParser.GetComponentParametersFromQueryParameters(typeof(ComponentMock), rawQueryParameters!);
        Assert.Equal(boolValue, componentParameters[nameof(ComponentMock.BoolParameter)]);
    }

    [Fact]
    public void CombinedParsing_String()
    {
        var uri = "https://example.com/path";
        var stringValue = "string";
        var queryParameters = new Dictionary<string, object?>()
        {
            { "customStringParameter", stringValue }
        };

        var uriWithQueryParameters = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        var rawQueryParameters = UriParser.GetQueryParametersFromUri(uriWithQueryParameters);

        var componentParameters = ComponentParameterParser.GetComponentParametersFromQueryParameters(typeof(ComponentMock), rawQueryParameters!);
        Assert.Equal(stringValue, componentParameters[nameof(ComponentMock.StringParameter)]);
    }
}
