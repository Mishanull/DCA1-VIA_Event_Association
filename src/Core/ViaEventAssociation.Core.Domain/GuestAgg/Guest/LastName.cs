using ViaEventAssociation.Core.Domain.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.GuestAgg.Guest;
public class LastName : ValueObject
{
    internal string Value { get; set; }

    private LastName(string value)
    {
        Value = value;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static Result<LastName> Create(string value)
    {
        return Validate(new LastName(value));
    }

    private static Result<LastName> Validate(LastName lastName)
    {
        var result = new Result<LastName>(lastName);
        if (lastName.Value.Length < 2  || lastName.Value.Length > 25)
        {
            result.CollectError(ErrorHelper.CreateVeaError("Last name value should be between 2 and 25 characters.", ErrorType.ValidationFailed));
        }

        if (!lastName.Value.All(Char.IsLetter))
        {
            result.CollectError(ErrorHelper.CreateVeaError("The last name must only contain letters.", ErrorType.ValidationFailed));
        }

        return result;
    }
}
