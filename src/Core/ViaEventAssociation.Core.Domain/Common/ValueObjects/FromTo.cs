using ViaEventAssociation.Core.Domain.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.Common.ValueObjects;

public class FromTo : ValueObject
{
    public DateTime Start { get; init; }
    public DateTime End { get; init; }
    
    private FromTo(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    public static Result<FromTo> Create(DateTime start, DateTime end)
    {
        return Validate(new FromTo(start, end));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }
    
    private static Result<FromTo> Validate(FromTo fromTo)
    {
        var errorResult = new Result<FromTo>(fromTo);
        // start date is before end date
        if (fromTo.Start > fromTo.End)
        {
            errorResult.CollectError(new VeaError(ErrorType.InvalidFromTo, new ErrorMessage("Start date must be before end date")));
        }
        return errorResult;
    }
}