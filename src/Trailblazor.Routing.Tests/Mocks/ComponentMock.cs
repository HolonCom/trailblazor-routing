using Microsoft.AspNetCore.Components;

namespace Trailblazor.Routing.Tests.Mocks;

internal sealed class ComponentMock : ComponentBase
{
    [Parameter]
    [QueryParameter("customStringParameter")]
    public string? StringParameter { get; set; }

    [Parameter]
    [QueryParameter("customBoolParameter")]
    public bool BoolParameter { get; set; }

    [Parameter]
    [QueryParameter("customGuidParameter")]
    public Guid GuidParameter { get; set; }

    [Parameter]
    [QueryParameter("customTimeOnlyParameter")]
    public TimeOnly TimeOnlyParameter { get; set; }

    [Parameter]
    [QueryParameter("customDateOnlyParameter")]
    public DateOnly DateOnlyParameter { get; set; }

    [Parameter]
    [QueryParameter("customDateTimeParameter")]
    public DateTime DateTimeParameter { get; set; }

    [Parameter]
    [QueryParameter("customIntParameter")]
    public int IntParameter { get; set; }

    [Parameter]
    [QueryParameter("customDoubleParameter")]
    public double DoubleParameter { get; set; }

    [Parameter]
    [QueryParameter("customLongParameter")]
    public long LongParameter { get; set; }

    [Parameter]
    [QueryParameter("customDecimalParameter")]
    public decimal DecimalParameter { get; set; }

    [Parameter]
    [QueryParameter("differentParameterName")]
    public int ValidQueryParameterWithCustomName { get; set; }

    [QueryParameter("missingParameterAttribute")]
    public int InvalidQueryParameterWithMissingParameterAttribute { get; set; }
}
