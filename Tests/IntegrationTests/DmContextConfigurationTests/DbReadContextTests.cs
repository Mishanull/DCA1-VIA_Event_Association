using IntegrationTests.Utils;

namespace IntegrationTests.DmContextConfigurationTests;

public class DbReadContextTests
{
    [Fact]
    public async Task DbSeedTest()
    {
        await using var ctx = DbContextHelper.SetupReadContext().Seed();
        
        Assert.NotEmpty(ctx.Guests);
        Assert.Equal(50, ctx.Guests.Count());
        
        Assert.NotEmpty(ctx.Events);
        Assert.Equal(28, ctx.Events.Count());
        
        Assert.NotEmpty(ctx.Locations);
        Assert.Equal(9, ctx.Locations.Count());
    }
}