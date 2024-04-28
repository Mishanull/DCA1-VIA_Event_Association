using IntegrationTests.Utils;
using Microsoft.EntityFrameworkCore;
using UnitTests.FakeServices;
using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.LocationAgg;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.CreatorAggPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.EventAggPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.LocationAggPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.UnitOfWork;
using FakeCurrentTime = IntegrationTests.Utils.FakeCurrentTime;

namespace IntegrationTests.RepositoryTests;

public class EventRepositoryTests
{
    [Fact]
    public async Task SaveAndRetrieveVeaEvent()
    {
        //Arrange
        var ctx = DbContextHelper.SetupContext();
        var veaEventRepository = new EventSqliteRepositoryEfc(ctx);
        var creatorRepository = new CreatorSqliteRepositoryEfc(ctx);
        var creator = Creator.Create(Email.Create("benjamin.bartosik@gmail.com", new FakeEmailCheck()).Value).Value;
        var veaEvent = VeaEvent.Create(creator!.Id, new FakeCurrentTime()).Value;
        var title = Title.Create("Test title").Value!;
        veaEvent!.UpdateTitle(title);
        
        await creatorRepository.AddAsync(creator);
        await veaEventRepository.AddAsync(veaEvent);
        await ctx.SaveChangesAsync();
        
        //Act
        var retrievedResult = await veaEventRepository.FindAsync(veaEvent.Id);
        var retrievedVeaEvent = retrievedResult.Value;
        
        //Assert
        Assert.NotNull(retrievedVeaEvent);
        Assert.Equal(title.Value, retrievedVeaEvent.Title.Value);
        
        ctx.ChangeTracker.Clear();
    }
    
    [Fact]
    public async Task SaveVeaEventWithInvalidCreatorIdFails()
    {
        //Arrange
        var ctx = DbContextHelper.SetupContext();
        var uow = new SqliteUnitOfWork(ctx);
        var veaEventRepository = new EventSqliteRepositoryEfc(ctx);
        var invalidCreatorId = TId.Create<CreatorId>();
        var veaEvent = VeaEvent.Create(invalidCreatorId, new FakeCurrentTime()).Value;
        
        //Act
        await veaEventRepository.AddAsync(veaEvent!);

        //Assert
        await Assert.ThrowsAsync<DbUpdateException>(()=> uow.SaveChangesAsync());
        ctx.ChangeTracker.Clear();
    }
    
    [Fact]
    public async Task SaveVeaEventWithInvalidLocationIdFails()
    {
        //Arrange
        var ctx = DbContextHelper.SetupContext();
        var veaEventRepository = new EventSqliteRepositoryEfc(ctx);
        var creatorRepository = new CreatorSqliteRepositoryEfc(ctx);
        var creator = Creator.Create(Email.Create("benjamin.bartosik@gmail.com", new FakeEmailCheck()).Value).Value;
        var veaEvent = VeaEvent.Create(creator!.Id, new FakeCurrentTime()).Value;
        var invalidLocationId = TId.Create<LocationId>();
        veaEvent!.SetLocationId(invalidLocationId);
        
        //Act
        await creatorRepository.AddAsync(creator);
        await veaEventRepository.AddAsync(veaEvent);
        
        //Assert
        await Assert.ThrowsAsync<DbUpdateException>(() => ctx.SaveChangesAsync());
        
        ctx.ChangeTracker.Clear();
    }
    
    [Fact]
        public async Task SaveVeaEventWithValidLocationId()
        {
            //Arrange
            var ctx = DbContextHelper.SetupContext();
            var veaEventRepository = new EventSqliteRepositoryEfc(ctx);
            var creatorRepository = new CreatorSqliteRepositoryEfc(ctx);
            var locationRepository = new LocationSqliteRepositoryEfc(ctx);
            var creator = Creator.Create(Email.Create("benjamin.bartosik@gmail.com", new FakeEmailCheck()).Value).Value;
            var veaEvent = VeaEvent.Create(creator!.Id, new FakeCurrentTime()).Value;
            var location = Location.Create(LocationName.Create("LocationName").Value!, creator!.Id).Value;
            veaEvent!.SetLocationId(location!.Id);
            
            await creatorRepository.AddAsync(creator);
            await locationRepository.AddAsync(location);
            await veaEventRepository.AddAsync(veaEvent);
            await ctx.SaveChangesAsync();
            
            //Act
            var retrievedVeaEvent = (await veaEventRepository.FindAsync(veaEvent.Id)).Value;
            
            //Assert
            Assert.NotNull(retrievedVeaEvent);
            Assert.Equal(location.Id, retrievedVeaEvent.LocationId);
            
            ctx.ChangeTracker.Clear();
        }
}