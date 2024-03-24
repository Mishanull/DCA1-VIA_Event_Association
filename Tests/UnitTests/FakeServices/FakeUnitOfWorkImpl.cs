using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;

namespace UnitTests.FakeServices;

public class FakeUnitOfWorkImpl : IUnitOfWork
{
    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }
}