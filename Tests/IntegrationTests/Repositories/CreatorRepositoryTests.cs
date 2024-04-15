using IntegrationTests.Utils;
using UnitTests.FakeServices;
using UnitTests.Utils;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.CreatorAggPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.EventAggPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.GuestAggPersistence;

namespace IntegrationTests.Repositories;

public class CreatorRepositoryTests
{
    [Fact]
    public async Task SaveAndRetrieveCreator()
    {
        //Arrange
        var ctx = DbContextHelper.SetupContext();
        var creatorRepository = new CreatorSqliteRepositoryEfc(ctx);
        var email = Email.Create("benjamin.bartosik@gmail.com", new FakeEmailCheck()).Value;
        var creator = Creator.Create(email!).Value;

        await creatorRepository.AddAsync(creator!);
        await ctx.SaveChangesAsync();

        //Act
        var retrievedResult = await creatorRepository.FindAsync(creator!.Id);
        var retrievedCreator = retrievedResult.Value;

        //Assert
        Assert.NotNull(retrievedCreator);
        Assert.Equal(email, retrievedCreator.Email);
    }

    [Fact]
    public async Task SaveAndRetrieveInvite()
    {
        //Arrange
        var ctx = DbContextHelper.SetupContext();
        var creatorRepository = new CreatorSqliteRepositoryEfc(ctx);
        var veaEventRepository = new EventSqliteRepositoryEfc(ctx);
        var guestRepository = new GuestSqliteRepositoryEfc(ctx);
        var email = Email.Create("benjamin.bartosik@gmail.com", new FakeEmailCheck()).Value;
        var creator = Creator.Create(email!).Value!;
        var veaEvent = VeaEvent.Create(creator.Id, new FakeCurrentTime()).Value!;
        var guest = VeaGuest.Create(Email.Create("email@via.dk", new FakeEmailCheck()).Value!,
            FirstName.Create("john").Value!, LastName.Create("doe").Value!, new PictureUrl()).Value!;
        await guestRepository.AddAsync(guest);
        await creatorRepository.AddAsync(creator);
        await veaEventRepository.AddAsync(veaEvent);
        
        var invite = Invite.Create(guest.Id, creator.Id, veaEvent.Id).Value!;
        creator.AddInvite(invite);
        await ctx.SaveChangesAsync();

        //Act
        var retrievedResult = await creatorRepository.FindInviteAsync(invite!.Id);
        var retrievedInvite = retrievedResult.Value;
        
        //Assert
        Assert.NotNull(retrievedInvite);
        Assert.Equal(invite, retrievedInvite);
    }
}