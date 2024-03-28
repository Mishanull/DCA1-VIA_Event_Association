namespace ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;

public interface IUnitOfWork
{
    public Task SaveChangesAsync();
}