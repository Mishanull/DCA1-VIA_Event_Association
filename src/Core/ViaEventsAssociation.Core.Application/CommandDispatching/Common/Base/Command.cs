namespace ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;

public abstract class Command
{
    public override bool Equals(object? other)
    {
        if (other is null) return false;
        if (other.GetType() != GetType()) return false;
        return ((Command)other).GetEqualityComponents().SequenceEqual(GetEqualityComponents());
    }

    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(obj => obj?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }
}