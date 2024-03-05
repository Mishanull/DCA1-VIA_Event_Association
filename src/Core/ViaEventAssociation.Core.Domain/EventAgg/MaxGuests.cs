using ViaEventAssociation.Core.Domain.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.EventAgg;

public class MaxGuests : ValueObject
{
    internal int Value { get; init; }
    //private constructor
    private MaxGuests(int value)
    {
        Value = value;
    }
    //factory methods
    public static Result Create(int value)
    {
        return Validate(new MaxGuests(value));
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
    
    private static Result<MaxGuests> Validate(MaxGuests maxGuests)
    {
        var errorResult = new Result<MaxGuests>(maxGuests);
        switch (maxGuests.Value)
        {
            // maxGuests is at least 5
            case < 5:
                errorResult.CollectError(new VeaError(ErrorType.InvalidMaxGuests, new ErrorMessage("Max guests must be at least 5")));
                break;
            // maxGuests is less than or equal to 50
            case > 50:
                errorResult.CollectError(new VeaError(ErrorType.InvalidMaxGuests, new ErrorMessage("Max guests must be less than or equal to 50")));
                break;
        }
        // new maxGuests is bigger than the previous one
        // maxGuests is less than the location's capacity
        // these are not validation errors, but business rules ->ToDo: add business rules
        return errorResult;
    }
}