using ViaEventAssociation.Core.Domain.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.GuestAgg.Guest;

public class FirstName : ValueObject
{
    internal string Value { get; set; }

    private FirstName(string value)
    {
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static Result<FirstName> Create(string value)
    {
        return Validate(new FirstName(value));
    }

    private static Result<FirstName> Validate(FirstName firstName)
    {
        var result = new Result<FirstName>(firstName);
        if (firstName.Value.Length < 2 || firstName.Value.Length > 25)
        {
            result.CollectError(ErrorHelper.CreateVeaError("First name value should be between 2 and 25 characters.",
                ErrorType.ValidationFailed));
        }

        if (!firstName.Value.All(Char.IsLetter))
        {
            result.CollectError(ErrorHelper.CreateVeaError("The first name must only contain letters.",
                ErrorType.ValidationFailed));
        }

        return result;
    }
}