using IntegrationTests.Utils;
using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.EntityM_Trial;

namespace IntegrationTests.DmContextConfigurationTests;

public class DbContextTests
{
    [Fact]
    public async Task StrongIdAsPk()
    {
        await using var ctx = DbContextHelper.SetupContext();

        var id = TId.Create<MId>();
        EntityM entity = new(id);
    
        await DbContextHelper.SaveAndClearAsync(entity, ctx);

        var retrieved = ctx.EntityMs.SingleOrDefault(x => x.Id.Equals(id));
        Assert.NotNull(retrieved);
    }
}