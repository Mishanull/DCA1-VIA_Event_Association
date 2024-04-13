using IntegrationTests.Utils;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UnitTests.FakeServices;
using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.LocationAgg;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.CreatorAggPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.LocationAggPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.UnitOfWork;

namespace IntegrationTests.Repositories;

public class LocationRepositoryTests
{
    [Fact]
    public async Task SaveAndRetrieveLocation()
    {
        //Arrange
        var ctx = DbContextHelper.SetupContext();
        var locationRepository = new LocationSqliteRepositoryEfc(ctx);
        var creatorRepository = new CreatorSqliteRepositoryEfc(ctx);
        var name = LocationName.Create("LocationName").Value;
        var creator = Creator.Create(Email.Create("benjamin.bartosik@gmail.com", new FakeEmailCheck()).Value).Value;
        var location = Location.Create(name!, creator!.Id).Value;
        
        await creatorRepository.AddAsync(creator);
        await locationRepository.AddAsync(location!);
        await ctx.SaveChangesAsync();
        
        //Act
        var retrievedResult = await locationRepository.FindAsync(location!.Id);
        var retrievedLocation = retrievedResult.Value;
        
        //Assert
        Assert.NotNull(retrievedLocation.Name);
        Assert.Equal(name.Value, retrievedLocation.Name.Value);
        
        ctx.ChangeTracker.Clear();
    }

    [Fact]
    public async Task SaveLocationWithInvalidCreatorIdFails()
    {
        //Arrange
        var ctx = DbContextHelper.SetupContext();
        var uow = new SqliteUnitOfWork(ctx);
        var locationRepository = new LocationSqliteRepositoryEfc(ctx);
        var name = LocationName.Create("LocationName").Value;
        var invalidCreatorId = TId.Create<CreatorId>();
        var location = Location.Create(name!, invalidCreatorId).Value;
        
        //Act
        await locationRepository.AddAsync(location!);

        //Assert
        await Assert.ThrowsAsync<DbUpdateException>(()=> uow.SaveChangesAsync());
        ctx.ChangeTracker.Clear();
    }
}