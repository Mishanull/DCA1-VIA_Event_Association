using IntegrationTests.Utils;
using UnitTests.FakeServices;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.GuestAggPersistence;

namespace IntegrationTests.Repositories;

public class GuestRepositoryTests
{
    [Fact]
    public async Task SaveAndRetrieveGuest()
    {
        //Arrange
        var ctx = DbContextHelper.SetupContext();
        var guestRepository = new GuestSqliteRepositoryEfc(ctx);

        var email = Email.Create("bbb@gmail.com", new FakeEmailCheck()).Value;
        var firstName = FirstName.Create("Benjamin").Value;
        var lastName = LastName.Create("Bartosik").Value;
        var pictureUrl = PictureUrl.Create("url").Value;
        var guest = VeaGuest.Create(email!, firstName!, lastName!, pictureUrl!).Value;
        
        await guestRepository.AddAsync(guest!);
        await ctx.SaveChangesAsync();
        
        //Act
        var retrievedResult = await guestRepository.FindAsync(guest!.Id);
        var retrievedGuest = retrievedResult.Value;
        
        //Assert
        Assert.NotNull(retrievedGuest);
        Assert.Equal(email!.Value, retrievedGuest.Email!.Value);
        
        ctx.ChangeTracker.Clear();
    }
}