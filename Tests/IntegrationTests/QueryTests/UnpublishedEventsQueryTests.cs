using IntegrationTests.Utils;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries.QueryHandlers;

namespace IntegrationTests.QueryTests;

public class UnpublishedEventsQueryTests
{
    [Fact]
    public async void FetchUnpublishedEvents()
    {
        //Arrange
        await using var ctx = DbContextHelper.SetupReadContext().Seed();
        var query = new UnpublishedEventsPage.Query("bb99156b-67a9-46d1-9b3e-8584f7f827c3"); //from VeaDbContext.Seed() -method
        var handler = new UnpublishedEventsPageQueryHandler(ctx);

        //Act
        var answer = await handler.HandleAsync(query);
        
        //Assert
        Assert.NotNull(answer);
        Assert.Equal(2, answer.CancelledEvents.Count);
        Assert.Equal(17, answer.ActiveEvents.Count);
        Assert.Equal(5, answer.DraftEvents.Count);
        Assert.Equal(4, answer.ReadyEvents.Count);
    }
}