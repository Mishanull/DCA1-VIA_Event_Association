using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;

namespace UnitTests.FakeServices.Repositories;

public class FakeUow : IUnitOfWork
{
    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }
}