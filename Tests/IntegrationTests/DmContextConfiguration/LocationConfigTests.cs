using IntegrationTests.Utils;
using UnitTests.FakeServices;
using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.LocationAgg;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence;

namespace IntegrationTests.DmContextConfiguration;

public class LocationConfigTests
{
    //NonNullableSinglePrimitiveValuedValueObject
    [Fact]
    public async Task LocationNameValueObjectRetrieved()
    {
        //Arrange
        var ctx = DbContextHelper.SetupContext();
        var name = LocationName.Create("LocationName").Value;
        var creator = Creator.Create(Email.Create("benjamin.bartosik@gmail.com", new FakeEmailCheck()).Value).Value;
        var location = Location.Create(name!, creator!.Id).Value;

        //Act
        await DbContextHelper.SaveAndClearAsync(creator, ctx);
        await DbContextHelper.SaveAndClearAsync(location!, ctx);
        var retrieved = ctx.Locations.Single(x => x.Id == location!.Id);
        
        //Assert
        Assert.NotNull(retrieved.Name);
        Assert.Equal(name.Value, retrieved.Name.Value);
    }
    
    //NonNullableSinglePrimitiveValuedValueObject_FailWhenNull
    [Fact]
    public async Task SaveLocationWithNullNameFails()
    {
        //Arrange
        await using WriteDbContext ctx = DbContextHelper.SetupContext();
        var creator = Creator.Create(Email.Create("benjamin.bartosik@gmail.com", new FakeEmailCheck()).Value).Value;
        var location = Location.Create(null, creator!.Id).Value;
    
        //Act
        await ctx.Set<Location>().AddAsync(location!);
        
        //Assert
        Assert.Throws<InvalidOperationException>(() => ctx.SaveChanges());
    }
    
    
    //StrongIdAsFk_ValidTarget
    [Fact]
    public async Task RetrieveCreatorOfLocationUsingForeignKeyReference()
    {
        //Arrange
        await using WriteDbContext ctx = DbContextHelper.SetupContext();
        var name = LocationName.Create("LocationName").Value;
        var creator = Creator.Create(Email.Create("benjamin.bartosik@gmail.com", new FakeEmailCheck()).Value).Value;
        await DbContextHelper.SaveAndClearAsync(creator!, ctx);
        
        var location = Location.Create(name!, creator!.Id).Value;
        await DbContextHelper.SaveAndClearAsync(location!, ctx);
        
        //Act
        Location retrievedLocation = ctx.Locations.Single(x => x.Id == location!.Id);
        Creator? retrievedCreator = ctx.Creators.SingleOrDefault(y => y.Id == retrievedLocation.CreatorId);

        //Assert
        Assert.NotNull(retrievedCreator);
    }
    
    //StrongIdAsFk_InvalidTarget
    [Fact]
    public async Task SaveLocationWithInvalidCreatorForeignKeyFails()
    {
        //Arrange
        await using WriteDbContext ctx = DbContextHelper.SetupContext();
        var name = LocationName.Create("LocationName").Value;
        var location = Location.Create(name!, TId.Create<CreatorId>()).Value;
        
        //Act
        await ctx.Set<Location>().AddAsync(location!);
        Action exp = () => ctx.SaveChanges();

        //Assert
        Assert.NotNull(Record.Exception(exp));
    }
}