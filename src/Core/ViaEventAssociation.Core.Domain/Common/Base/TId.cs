namespace ViaEventAssociation.Core.Domain.Common.Base;

public abstract class TId
{
    public Guid Id { get; } = Guid.NewGuid();
}