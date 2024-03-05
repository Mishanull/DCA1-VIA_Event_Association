using ViaEventAssociation.Core.Domain.Common.Base;

namespace ViaEventAssociation.Core.Domain.Common.GuestAgg;

public class Email (string value) : ValueObject
{
    public string Value { get; }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}