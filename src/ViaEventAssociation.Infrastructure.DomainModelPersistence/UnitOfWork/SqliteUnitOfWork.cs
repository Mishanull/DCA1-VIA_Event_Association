using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.UnitOfWork;

public class SqliteUnitOfWork(DbContext context):IUnitOfWork
{
    public Task SaveChangesAsync() => context.SaveChangesAsync();
}