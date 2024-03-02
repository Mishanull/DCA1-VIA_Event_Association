using ViaEventAssociation.Core.Domain.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.Common.ValueObjects;

public class TimeSpan : ValueObject
{
    private DateTime Start { get; init; }
    private DateTime End { get; init; }
    
    public TimeSpan(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
        Validate();
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }
    
    private ErrorResult Validate()
    {
        var errorResult = new ErrorResult();
        // start date is not in the past
        if (Start < DateTime.Now)
        {
            errorResult.CollectError(new VeaError(ErrorType.InvalidTimeSpan, new ErrorMessage("Start date cannot be in the past")));
        }
        // start date is before end date
        if (Start > End)
        {
            errorResult.CollectError(new VeaError(ErrorType.InvalidTimeSpan, new ErrorMessage("Start date must be before end date")));
        }
        return errorResult;
    }
}