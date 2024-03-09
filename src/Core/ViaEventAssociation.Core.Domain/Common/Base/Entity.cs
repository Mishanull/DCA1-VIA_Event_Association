namespace ViaEventAssociation.Core.Domain.Common.Base;

public class Entity<TId>
{
    public TId Id { get; init; }

    protected Entity(TId id)
    {
        Id = id;
    }
}