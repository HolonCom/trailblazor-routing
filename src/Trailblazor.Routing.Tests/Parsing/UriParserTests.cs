using Microsoft.Extensions.DependencyInjection;
using Trailblazor.Routing.Parsing;
using Trailblazor.Routing.Tests.DI;

namespace Trailblazor.Routing.Tests.Parsing;

public class UriParserTests
{
    private readonly IServiceProvider _serviceProvider = TestServiceProviderFactory.Create();
    private IUriParser UriParser => _serviceProvider.GetRequiredService<IUriParser>();

    [Fact]
    public void GetUriWithoutQueryParameters_RemovesQueryParameters()
    {
        var uri = "https://example.com/path?param1=value1&param2=value2";

        var result = UriParser.GetUriWithoutQueryParameters(uri);
        Assert.Equal("https://example.com/path", result);
    }

    [Theory]
    [InlineData("https://example.com/path", "https://example.com/path")]
    [InlineData("https://example.com/path?", "https://example.com/path")]
    [InlineData("https://example.com/path?param=", "https://example.com/path")]
    public void GetUriWithoutQueryParameters_HandlesEdgeCases(string uri, string expected)
    {
        var result = UriParser.GetUriWithoutQueryParameters(uri);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetUriWithQueryParameters_AddsQueryParameters()
    {
        var uri = "https://example.com/path";
        var queryParameters = new Dictionary<string, string?>()
        {
            { "param1", "value1" },
            { "param2", "value2" }
        };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.Equal("https://example.com/path?param1=value1&param2=value2", result);
    }

    [Fact]
    public void GetUriWithQueryParameters_AppendsToExistingQueryParameters()
    {
        var uri = "https://example.com/path?existingParam=existingValue";
        var queryParameters = new Dictionary<string, string?>()
        {
            { "newParam", "newValue" }
        };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.Equal("https://example.com/path?existingParam=existingValue&newParam=newValue", result);
    }

    [Fact]
    public void GetUriWithQueryParameters_HandlesEmptyQueryParameters()
    {
        var uri = "https://example.com/path";
        var queryParameters = new Dictionary<string, string?>();

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.Equal("https://example.com/path", result);
    }

    [Fact]
    public void GetUriWithQueryParameters_EncodesSpecialCharacters()
    {
        var uri = "https://example.com/path";
        var queryParameters = new Dictionary<string, string?>()
        {
            { "paramwithspace", "value with space" },
            { "specialchars", "value@chars" }
        };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.Equal("https://example.com/path?paramwithspace=value%20with%20space&specialchars=value%40chars", result);
    }

    [Fact]
    public void GetUriWithQueryParameters_HandlesNullValues()
    {
        var queryParameters = new Dictionary<string, object?> { { "nullableParameter", null } };

        var result = UriParser.GetUriWithQueryParameters("https://example.com/path", queryParameters);
        Assert.DoesNotContain("nullableParameter", result);
    }

    [Fact]
    public void GetQueryParametersFromUri_ExtractsQueryParameters()
    {
        var uri = "https://example.com/path?param1=value1&param2=value2";

        var result = UriParser.GetQueryParametersFromUri(uri);
        Assert.Equal(2, result.Count);
        Assert.Equal("value1", result["param1"]);
        Assert.Equal("value2", result["param2"]);
    }

    [Fact]
    public void GetQueryParametersFromUri_HandlesNoQueryParameters()
    {
        var uri = "https://example.com/path";

        var result = UriParser.GetQueryParametersFromUri(uri);
        Assert.Empty(result);
    }

    [Fact]
    public void GetQueryParametersFromUri_HandlesEmptyParameterValues()
    {
        var uri = "https://example.com/path?param1=&param2=value2";

        var result = UriParser.GetQueryParametersFromUri(uri);
        Assert.Equal(2, result.Count);
        Assert.Equal("", result["param1"]);
        Assert.Equal("value2", result["param2"]);
    }

    [Fact]
    public void GetQueryParametersFromUri_HandlesEncodedCharacters()
    {
        var uri = "https://example.com/path?param%20with%20space=value%20with%20space&special%40chars=value%40chars";

        var result = UriParser.GetQueryParametersFromUri(uri);
        Assert.Equal(2, result.Count);
        Assert.Equal("value with space", result["param with space"]);
        Assert.Equal("value@chars", result["special@chars"]);
    }

    [Fact]
    public void GetUriWithQueryParameters_GuidParameter()
    {
        var uri = "https://example.com/path";
        var queryParameters = new Dictionary<string, object?>()
        {
            { "guid", Guid.NewGuid() }
        };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        var guidValue = queryParameters["guid"]?.ToString();
        Assert.Contains($"guid={guidValue}", result);
    }

    [Fact]
    public void GetUriWithQueryParameters_NullableGuidParameter()
    {
        var uri = "https://example.com/path";
        Guid? nullableGuid = null;
        var queryParameters = new Dictionary<string, object?>()
        {
            { "nullableGuid", nullableGuid }
        };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.DoesNotContain("nullableGuid", result);
    }

    [Fact]
    public void GetUriWithQueryParameters_IntParameter()
    {
        var uri = "https://example.com/path";
        var queryParameters = new Dictionary<string, object?>()
        {
            { "int", 123 }
        };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.Contains("int=123", result);
    }

    [Fact]
    public void GetUriWithQueryParameters_NullableIntParameter()
    {
        var uri = "https://example.com/path";
        int? nullableInt = 456;
        var queryParameters = new Dictionary<string, object?>()
        {
            { "nullableInt", nullableInt }
        };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.Contains("nullableInt=456", result);
    }

    [Fact]
    public void GetUriWithQueryParameters_LongParameter()
    {
        var uri = "https://example.com/path";
        var queryParameters = new Dictionary<string, object?>()
        {
            { "long", 789123456789 }
        };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.Contains("long=789123456789", result);
    }

    [Fact]
    public void GetUriWithQueryParameters_NullableLongParameter()
    {
        var uri = "https://example.com/path";
        long? nullableLong = 987654321012;
        var queryParameters = new Dictionary<string, object?>()
        {
            { "nullableLong", nullableLong }
        };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.Contains("nullableLong=987654321012", result);
    }

    [Fact]
    public void GetUriWithQueryParameters_DoubleParameter()
    {
        var uri = "https://example.com/path";
        var queryParameters = new Dictionary<string, object?>()
        {
            { "double", 123.456 }
        };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.Contains("double=123.456", result);
    }

    [Fact]
    public void GetUriWithQueryParameters_NullableDoubleParameter()
    {
        var uri = "https://example.com/path";
        double? nullableDouble = 456.789;
        var queryParameters = new Dictionary<string, object?>()
        {
            { "nullableDouble", nullableDouble }
        };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.Contains("nullableDouble=456.789", result);
    }

    [Fact]
    public void GetUriWithQueryParameters_DecimalParameter()
    {
        var uri = "https://example.com/path";
        var queryParameters = new Dictionary<string, object?>()
        {
            { "decimal", 123.45m }
        };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.Contains("decimal=123.45", result);
    }

    [Fact]
    public void GetUriWithQueryParameters_NullableDecimalParameter()
    {
        var uri = "https://example.com/path";
        decimal? nullableDecimal = 678.90m;
        var queryParameters = new Dictionary<string, object?>()
        {
            { "nullableDecimal", nullableDecimal }
        };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.Contains("nullableDecimal=678.90", result);
    }

    [Fact]
    public void GetUriWithQueryParameters_DateTimeParameter()
    {
        var uri = "https://example.com/path";
        var dateTimeValue = new DateTime(2023, 11, 5, 13, 45, 30, DateTimeKind.Utc);
        var queryParameters = new Dictionary<string, object?>()
    {
        { "dateTime", dateTimeValue }
    };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.Contains("dateTime=2023-11-05T13%3A45%3A30Z", result); // Corrected expected value
    }

    [Fact]
    public void GetUriWithQueryParameters_NullableDateTimeParameter()
    {
        var uri = "https://example.com/path";
        DateTime? nullableDateTime = new DateTime(2023, 11, 5, 13, 45, 30, DateTimeKind.Utc);
        var queryParameters = new Dictionary<string, object?>()
    {
        { "nullableDateTime", nullableDateTime }
    };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.Contains("nullableDateTime=2023-11-05T13%3A45%3A30Z", result); // Corrected expected value
    }

    [Fact]
    public void GetUriWithQueryParameters_DateOnlyParameter()
    {
        var uri = "https://example.com/path";
        var dateOnlyValue = new DateOnly(2023, 11, 5);
        var queryParameters = new Dictionary<string, object?>()
        {
            { "dateOnly", dateOnlyValue }
        };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.Contains("dateOnly=2023-11-05", result);
    }

    [Fact]
    public void GetUriWithQueryParameters_NullableDateOnlyParameter()
    {
        var uri = "https://example.com/path";
        DateOnly? nullableDateOnly = new DateOnly(2023, 11, 5);
        var queryParameters = new Dictionary<string, object?>()
        {
            { "nullableDateOnly", nullableDateOnly }
        };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.Contains("nullableDateOnly=2023-11-05", result);
    }

    [Fact]
    public void GetUriWithQueryParameters_TimeOnlyParameter()
    {
        var uri = "https://example.com/path";
        var timeOnlyValue = new TimeOnly(13, 45, 30);
        var queryParameters = new Dictionary<string, object?>()
        {
            { "timeOnly", timeOnlyValue }
        };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.Contains("timeOnly=13%3A45%3A30", result);
    }

    [Fact]
    public void GetUriWithQueryParameters_NullableTimeOnlyParameter()
    {
        var uri = "https://example.com/path";
        TimeOnly? nullableTimeOnly = new TimeOnly(13, 45, 30);
        var queryParameters = new Dictionary<string, object?>()
        {
            { "nullableTimeOnly", nullableTimeOnly }
        };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.Contains("nullableTimeOnly=13%3A45%3A30", result);
    }

    [Fact]
    public void GetUriWithQueryParameters_StringParameter()
    {
        var uri = "https://example.com/path";
        var queryParameters = new Dictionary<string, object?>()
        {
            { "string", "value" }
        };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.Contains("string=value", result);
    }

    [Fact]
    public void GetUriWithQueryParameters_NullableStringParameter()
    {
        var uri = "https://example.com/path";
        string? nullableString = null;
        var queryParameters = new Dictionary<string, object?>()
        {
            { "nullableString", nullableString }
        };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.DoesNotContain("nullableString", result);
    }

    [Fact]
    public void GetUriWithQueryParameters_BoolParameter()
    {
        var uri = "https://example.com/path";
        var queryParameters = new Dictionary<string, object?>()
        {
            { "bool", true }
        };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.Contains("bool=True", result);
    }

    [Fact]
    public void GetUriWithQueryParameters_NullableBoolParameter()
    {
        var uri = "https://example.com/path";
        bool? nullableBool = null;
        var queryParameters = new Dictionary<string, object?>()
        {
            { "nullableBool", nullableBool }
        };

        var result = UriParser.GetUriWithQueryParameters(uri, queryParameters);
        Assert.DoesNotContain("nullableBool", result);
    }
}
