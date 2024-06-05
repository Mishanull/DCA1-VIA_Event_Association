using IntegrationTests.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ViaEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Infrastructure.EfcQueries.DTOs;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence;

namespace IntegrationTests.WebAPI;

internal class VeaWebApplicationFactory : WebApplicationFactory<Program>
{
    private IServiceCollection serviceCollection;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            serviceCollection = services;
            services.RemoveAll(typeof(DbContextOptions<WriteDbContext>));
            services.RemoveAll(typeof(DbContextOptions<VeaDbContext>));
            services.RemoveAll<WriteDbContext>();
            services.RemoveAll<VeaDbContext>();

            string connString = GetConnectionString();
            services.AddDbContext<WriteDbContext>(options =>
            {
                options.UseSqlite(connString);
            });

            services.AddDbContext<VeaDbContext>(options =>
            {
                options.UseSqlite(connString);
            });

            services.AddScoped<ICurrentTime, FakeCurrentTime>();

            SetupCleanDatabase(services);
        });
    }

    private void SetupCleanDatabase(IServiceCollection services)
    {
        WriteDbContext dmContext = services.BuildServiceProvider().GetService<WriteDbContext>()!;
        VeaDbContext veaDbContext = services.BuildServiceProvider().GetService<VeaDbContext>()!;
        veaDbContext.Seed();
        dmContext.Database.EnsureDeleted();
        dmContext.Database.EnsureCreated();
        veaDbContext.Database.EnsureDeleted();
        veaDbContext.Database.EnsureCreated();
    }

    private string GetConnectionString()
    {
        string testDbName = "Test" + Guid.NewGuid() + ".db";
        return "Data Source = " + testDbName;
    }

    protected override void Dispose(bool disposing)
    {
        WriteDbContext dmContext = serviceCollection.BuildServiceProvider().GetService<WriteDbContext>()!;
        dmContext.Database.EnsureDeleted();
        base.Dispose(disposing);
    }

}