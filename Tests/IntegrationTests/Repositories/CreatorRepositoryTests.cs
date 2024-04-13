using IntegrationTests.Utils;
using UnitTests.FakeServices;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.CreatorAggPersistence;

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
}