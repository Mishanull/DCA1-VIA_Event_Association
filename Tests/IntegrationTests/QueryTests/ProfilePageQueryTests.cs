using IntegrationTests.Utils;
using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries.QueryHandlers;

namespace IntegrationTests.QueryTests;

public class ProfilePageQueryTests
{
    [Fact]
    public async Task GuestsWithNoEventsReturnsNoEvents()
    {
        //Arrange
        await using var ctx = DbContextHelper.SetupReadContext().Seed();
        //get guest
        var guest = await ctx.Guests.FirstAsync();
        var query = new ProfilePage.Query(guest.Id,1,5);
        var handler = new ProfilePageQueryHandler(ctx);

        //Act
        var answer = await handler.HandleAsync(query);
        
        //Assert
        Assert.NotNull(answer);
        Assert.NotNull(answer.Guest);
        Assert.NotNull(answer.Events);
        Assert.Empty(answer.Events);
        Assert.Equal(0, answer.GuestPendingInvitesCount);
    }
}