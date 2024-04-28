using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using UnitTests.FakeServices;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.CreatorAggPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.EventAggPersistence;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Events;

namespace IntegrationTests.WebAPI.Events;

public class UpdateFromToTest
{
    [Fact]
        public async Task UpdateFromTo_ShouldReturnOk()
        {
            // Arrange
            await using WebApplicationFactory<Program> webApplicationFactory = new VeaWebApplicationFactory();
            HttpClient client = webApplicationFactory.CreateClient();

            var veaEvent = await SetupEventAsync(webApplicationFactory);
        
            UpdateFromToRequest request = new UpdateFromToRequest
            {
                EventId = veaEvent.Id.ToString(),
                From = DateTime.Today.AddDays(1).AddHours(10),
                To = DateTime.Today.AddDays(1).AddHours(11)
            };

            // Act
            HttpResponseMessage response = await client.PostAsync($"api/events/updateFromTo", JsonContent.Create(request));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task UpdateFromTo_WithInvalidEventId_ShouldReturnBadRequest()
        {
            // Arrange
            await using WebApplicationFactory<Program> webApplicationFactory = new VeaWebApplicationFactory();
            HttpClient client = webApplicationFactory.CreateClient();
        
            var veaEvent = await SetupEventAsync(webApplicationFactory);
        
            UpdateFromToRequest request = new UpdateFromToRequest
            {
                EventId = "bollocks",
                From = DateTime.Today.AddDays(1).AddHours(10),
                To = DateTime.Today.AddDays(1).AddHours(11)
            };

            // Act
            HttpResponseMessage response = await client.PostAsync($"api/events/updateFromTo", JsonContent.Create(request));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.True(!response.IsSuccessStatusCode);
        }
        
        [Fact]
        public async Task UpdateFromTo_WithInvalidFromTo_ShouldReturnBadRequest()
        {
            // Arrange
            await using WebApplicationFactory<Program> webApplicationFactory = new VeaWebApplicationFactory();
            HttpClient client = webApplicationFactory.CreateClient();
        
            var veaEvent = await SetupEventAsync(webApplicationFactory);
        
            UpdateFromToRequest request = new UpdateFromToRequest
            {
                EventId = veaEvent.Id.ToString(),
                From = new DateTime(1000,1,1),
                To = DateTime.Today.AddDays(1).AddHours(11)
            };

            // Act
            HttpResponseMessage response = await client.PostAsync($"api/events/updateFromTo", JsonContent.Create(request));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.True(!response.IsSuccessStatusCode);
        }
        
        private static async Task<VeaEvent> SetupEventAsync(WebApplicationFactory<Program> webApplicationFactory)
        {
            using var scope = webApplicationFactory.Services.CreateScope();
            var services = scope.ServiceProvider;
            var writeDbContext = services.GetRequiredService<WriteDbContext>();
            var currentTime = new FakeCurrentTime();
        
            var creatorRepository = new CreatorSqliteRepositoryEfc(writeDbContext);
            var eventRepository = new EventSqliteRepositoryEfc(writeDbContext);
            var creator = Creator.Create(Email.Create("123456@via.dk", new FakeEmailCheck()).Value).Value;
            var veaEvent = VeaEvent.Create(creator!.Id, currentTime).Value!;

            await eventRepository.AddAsync(veaEvent);
            await creatorRepository.AddAsync(creator);
            await writeDbContext.SaveChangesAsync();

            return veaEvent;
        }
}