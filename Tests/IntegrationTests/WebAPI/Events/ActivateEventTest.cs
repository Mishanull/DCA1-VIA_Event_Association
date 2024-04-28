using System.Net;
using IntegrationTests.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using UnitTests.FakeServices;
using UnitTests.FakeServices.Repositories;
using UnitTests.Utils;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.LocationAgg;
using ViaEventAssociation.Infrastructure.EfcQueries.DTOs;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.CreatorAggPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.EventAggPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.LocationAggPersistence;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Events;
using ViaEventsAssociation.Core.Application.CommandHandler.Handlers.Event;
using Creator = ViaEventAssociation.Core.Domain.CreatorAgg.Creator;
using FakeCurrentTime = IntegrationTests.Utils.FakeCurrentTime;
using Location = ViaEventAssociation.Core.Domain.LocationAgg.Location;

namespace IntegrationTests.WebAPI.Events;

public class ActivateEventTest
{

    [Fact]
    public async Task ActivateEvent_ShouldReturnOk()
    {
        // Arrange
        await using WebApplicationFactory<Program> webApplicationFactory = new VeaWebApplicationFactory();
        HttpClient client = webApplicationFactory.CreateClient();
        
        using var scope = webApplicationFactory.Services.CreateScope();
        var services = scope.ServiceProvider;
        var writeDbContext = services.GetRequiredService<WriteDbContext>();
        
        var creatorRepository = new CreatorSqliteRepositoryEfc(writeDbContext);
        var locationRepository = new LocationSqliteRepositoryEfc(writeDbContext);
        var eventRepository = new EventSqliteRepositoryEfc(writeDbContext);
        
        var name = LocationName.Create("LocationName").Value;
        var creator = Creator.Create(Email.Create("123456@via.dk", new FakeEmailCheck()).Value).Value;
        var location = Location.Create(name!, creator!.Id).Value;
        
        var veaEventBuilder = new VeaEventBuilder();
        var veaEvent = veaEventBuilder.Init()
            .WithTitle("Test Event")
            .WithDescription("Test Description")
            //from 10:00 tmrw until 11:00 tmrw *Today returns 00:00
            .WithFromTo( DateTime.Today.AddDays(1).AddHours(10), DateTime.Today.AddDays(1).AddHours(11))
            .WithCreatorId(creator.Id.ToString())
            .WithStatus(VeaEventStatus.Ready)
            .Build(); //event cannot be activated unless these fields are set!
        
        await creatorRepository.AddAsync(creator);
        await locationRepository.AddAsync(location!);
        await eventRepository.AddAsync(veaEvent);
        await writeDbContext.SaveChangesAsync();
        // debug
        var testEvent = await eventRepository.FindAsync(veaEvent.Id);
        
        var request = new ActivateEventRequest(veaEvent.Id.ToString());
        
        // Act
        HttpResponseMessage response = await client.PostAsync($"api/events/{request.EventId}/activate", JsonContent.Create(new {}));
        
        //Assert
        
        Assert.True(response.IsSuccessStatusCode);
        Assert.True(response.StatusCode == HttpStatusCode.OK);
        
        
    }
    
}