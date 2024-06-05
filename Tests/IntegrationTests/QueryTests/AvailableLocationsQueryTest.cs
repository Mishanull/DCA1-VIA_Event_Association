using IntegrationTests.Utils;
using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries.QueryHandlers;

namespace IntegrationTests.QueryTests;

public class AvailableLocationsQueryTest
{
    [Fact]
    public async Task AvailableLocationsQuery_LocationsAddedToCup_Success()
    {
        //Arrange
        await using var ctx = DbContextHelper.SetupReadContext().Seed();
        //get guest
        var veaEvent = await ctx.Events.FirstAsync();
        var query = new AvailableLocationsPage.Query(veaEvent.Id);
        var handler = new AvailableLocationsPageQueryHandler(ctx);

        //Act
        var answer = await handler.HandleAsync(query);
        
        //Assert
        Assert.NotNull(answer);
        Assert.NotNull(answer.Event);
        Assert.NotNull(answer.CreatorEmail);
        Assert.NotNull(answer.Locations);
        Assert.NotEmpty(answer.Locations);
    }
}