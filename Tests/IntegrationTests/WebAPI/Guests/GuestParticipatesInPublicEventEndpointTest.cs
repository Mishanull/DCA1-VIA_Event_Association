using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using UnitTests.FakeServices;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.CreatorAggPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.EventAggPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.GuestAggPersistence;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Guests;


namespace IntegrationTests.WebAPI.Guests;

public class GuestParticipatesInPublicEventEndpointTest
{
    [Fact]
    public async Task GuestParticipatesInPublicEvent_Success()
    {
        //Arrange
        await using WebApplicationFactory<Program> webApplicationFactory = new VeaWebApplicationFactory();
        HttpClient client = webApplicationFactory.CreateClient();
        var setupResult = await SetupEventAsync(webApplicationFactory);
        var veaEvent = setupResult.veaEvent;
        var guest = setupResult.guest;
        var request = new GuestParticipatesInPublicEventRequest(veaEvent.Id.ToString(), guest.Id.ToString(),
            "I wanna participate fam.");

        //Act
        HttpResponseMessage responseMessage = await client.PostAsync("api/guest/participate-public-event", JsonContent.Create(request));
        
        //Assert
        Assert.Equal(HttpStatusCode.OK,responseMessage.StatusCode);
    } 
    
    [Fact]
    public async Task GuestParticipatesInPublicEvent_InvalidEventId_Failure()
    {
        //Arrange
        await using WebApplicationFactory<Program> webApplicationFactory = new VeaWebApplicationFactory();
        HttpClient client = webApplicationFactory.CreateClient();
        var setupResult = await SetupEventAsync(webApplicationFactory);
        var veaEvent = setupResult.veaEvent;
        var guest = setupResult.guest;
        var request = new GuestParticipatesInPublicEventRequest("baloney", guest.Id.ToString(),
            "I wanna participate fam.");

        //Act
        HttpResponseMessage responseMessage = await client.PostAsync("api/guest/participate-public-event", JsonContent.Create(request));
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest,responseMessage.StatusCode);
    }
    
    private static async Task<(VeaEvent veaEvent, VeaGuest guest)> SetupEventAsync(WebApplicationFactory<Program> webApplicationFactory)
    {
        using var scope = webApplicationFactory.Services.CreateScope();
        var services = scope.ServiceProvider;
        var writeDbContext = services.GetRequiredService<WriteDbContext>();
        var currentTime = new FakeCurrentTime();

        var creatorRepository = new CreatorSqliteRepositoryEfc(writeDbContext);
        var eventRepository = new EventSqliteRepositoryEfc(writeDbContext);
        var guestRepository = new GuestSqliteRepositoryEfc(writeDbContext);
        var creator = Creator.Create(Email.Create("123456@via.dk", new FakeEmailCheck()).Value!).Value;
        var guest = VeaGuest.Create(Email.Create("123456@via.dk", new FakeEmailCheck()).Value!,
            FirstName.Create("David").Value!, LastName.Create("Mihai").Value!, new PictureUrl());
        var veaEvent = VeaEvent.Create(creator!.Id, currentTime).Value!;
        veaEvent.MakePublic();
        veaEvent.Activate();
        
        await eventRepository.AddAsync(veaEvent);
        await creatorRepository.AddAsync(creator);
        await guestRepository.AddAsync(guest.Value!);
        await writeDbContext.SaveChangesAsync();

        return (veaEvent, guest.Value!);
    }
}