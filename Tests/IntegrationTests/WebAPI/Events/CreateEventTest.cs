using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using UnitTests.FakeServices;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.LocationAgg;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.CreatorAggPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.LocationAggPersistence;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Events;
using Creator = ViaEventAssociation.Core.Domain.CreatorAgg.Creator;


namespace IntegrationTests.WebAPI.Events;

public class CreateEventTest
{
    [Fact]
    public async Task CreateEvent_ShouldReturnOk()
    {
        // Arrange
        await using WebApplicationFactory<Program> webApplicationFactory = new VeaWebApplicationFactory();
        HttpClient client = webApplicationFactory.CreateClient();
        
        using var scope = webApplicationFactory.Services.CreateScope();
        var services = scope.ServiceProvider;
        var writeDbContext = services.GetRequiredService<WriteDbContext>();
        
        var creatorRepository = new CreatorSqliteRepositoryEfc(writeDbContext);
        var creator = Creator.Create(Email.Create("123456@via.dk", new FakeEmailCheck()).Value).Value;
        await creatorRepository.AddAsync(creator);
        await writeDbContext.SaveChangesAsync();
        
        CreateEventRequest request = new CreateEventRequest(creator.Id.ToString()!);
        
        // Act
        HttpResponseMessage response = await client.PostAsync($"api/events/create", JsonContent.Create(request));
        
        // Assert
        Assert.True(response.IsSuccessStatusCode); 
        Assert.True(response.StatusCode == HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task CreateEvent_WrongCreatorId_ShouldReturnBadRequest()
    {
        await using VeaWebApplicationFactory webApplicationFactory = new VeaWebApplicationFactory();
        HttpClient client = webApplicationFactory.CreateClient();

        var creatorId = "boollocks";
        CreateEventRequest request = new CreateEventRequest(creatorId);
        HttpResponseMessage response = await client.PostAsync($"/api/events/create", JsonContent.Create(request));
        
        Assert.True(!response.IsSuccessStatusCode); 
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
    }
}