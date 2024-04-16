using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Infrastructure.EfcQueries.DTOs;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence;

namespace IntegrationTests.Utils;

public static class DbContextHelper
{
    public static WriteDbContext SetupContext()
    {
        DbContextOptionsBuilder<WriteDbContext> optionsBuilder = new();
        var testDbName = "Test" + Guid.NewGuid() +".db";
        optionsBuilder.UseSqlite(@"Data Source = " + testDbName);
        WriteDbContext context = new(optionsBuilder.Options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return context;
    }
    
    public static async Task SaveAndClearAsync<T>(T entity, WriteDbContext context) 
        where T : class
    {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
    }

    public static VeaDbContext SetupReadContext()
    {
        DbContextOptionsBuilder<VeaDbContext> optionsBuilder = new();
        string testDbName = "Test" + Guid.NewGuid() + ".db";
        optionsBuilder.UseSqlite(@"Data Source = " + testDbName);
        VeaDbContext context = new(optionsBuilder.Options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        
        return context;
    }
}