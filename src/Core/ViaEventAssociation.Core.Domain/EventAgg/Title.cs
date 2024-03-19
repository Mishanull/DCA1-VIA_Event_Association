using ViaEventAssociation.Core.Domain.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.EventAgg;

public class Title : ValueObject
{
    internal string? Value { get; }
    //private constructor
    private Title(string? value)
    {
        Value = value;
    }
    //factory method
    public static Result<Title> Create(string title)
    {
        return Validate(new Title(title));
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    private static Result<Title> Validate(Title title)
    {
        var errorResult = new Result<Title>(title);
        if (string.IsNullOrWhiteSpace(title.Value) || title.Value.Length < 3 || title.Value.Length > 75)
        {
            errorResult.CollectError(new VeaError(ErrorType.InvalidTitle, new ErrorMessage("Title length must be between 3 and 75 characters")));
        }
        return errorResult;
    }
}