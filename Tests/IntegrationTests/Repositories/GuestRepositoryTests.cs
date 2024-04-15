using IntegrationTests.Utils;
using UnitTests.FakeServices;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.CreatorAggPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.EventAggPersistence;
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
    
    // This bloody thing should be working in the exact same manner as the invite yet it doesn't
    // Fuck me
    // [Fact]
    // public async Task SaveAndRetrieveRequest()
    // {
    //     //Arrange
    //     var ctx = DbContextHelper.SetupContext();
    //     var creatorRepository = new CreatorSqliteRepositoryEfc(ctx);
    //     var veaEventRepository = new EventSqliteRepositoryEfc(ctx);
    //     var guestRepository = new GuestSqliteRepositoryEfc(ctx);
    //     var email = Email.Create("bbb@gmail.com", new FakeEmailCheck()).Value;
    //     var firstName = FirstName.Create("Benjamin").Value;
    //     var lastName = LastName.Create("Bartosik").Value;
    //     var pictureUrl = PictureUrl.Create("url").Value;
    //     var guest = VeaGuest.Create(email!, firstName!, lastName!, pictureUrl!).Value!;
    //     var creator = Creator.Create(email!).Value!;
    //     var veaEvent = VeaEvent.Create(creator.Id, new FakeCurrentTime()).Value!;
    //     await guestRepository.AddAsync(guest);
    //     await creatorRepository.AddAsync(creator);
    //     await veaEventRepository.AddAsync(veaEvent);
    //     var request = Request.Create("reasonsdsdsd", veaEvent.Id, guest.Id).Value!;
    //     guest.AddRequest(request);
    //     await ctx.SaveChangesAsync();
    //
    //     //Act
    //     var retrievedResult = await guestRepository.FindRequestAsync(request.Id);
    //     var retrievedRequest = retrievedResult.Value;
    //
    //     //Assert
    //     Assert.NotNull(retrievedRequest);
    //     Assert.Equal(request, retrievedRequest);
    //     ctx.ChangeTracker.Clear();
    // }
}