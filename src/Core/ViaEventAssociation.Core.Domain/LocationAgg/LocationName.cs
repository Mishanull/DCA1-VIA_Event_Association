using ViaEventAssociation.Core.Domain.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.LocationAgg;

public class LocationName : ValueObject
{
    internal LocationName(string value)
    {
        Value = value;
    }
    private LocationName(){} // For EF Core
    public string Value { get; }

    public static Result<LocationName> Create(string value)
    {
        var result = new Result<LocationName>(null);
        result.CollectErrors(Validate(value).Errors);
        if (result.IsErrorResult())
        {
            return result;
        }

        return new Result<LocationName>(new LocationName(value));
    }

    private static Result Validate(string value)
    {
        var result = new Result();
        if (value.Length > 120 || value.Length < 5)
        {
            result.CollectError(ErrorHelper.CreateVeaError("Validation failed for location name. The range for the length is 5-120.", ErrorType.ValidationFailed));
        }

        return result;
    }
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}