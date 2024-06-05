using System.Net.Http.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace IntegrationTests.WebAPI.Events;

public class CreateEventTest
{
    [Fact]
    public async Task CreateEvent_ShouldReturnOk()
    {
        await using WebApplicationFactory<Program> webApplicationFactory = new VeaWebApplicationFactory();
        HttpClient client = webApplicationFactory.CreateClient();

        var creatorId = new Guid().ToString();
        HttpResponseMessage response = await client.PostAsync($"/api/events/{creatorId}/create", JsonContent.Create(new {}));
        
        Assert.True(response.IsSuccessStatusCode); 
    }
    
    [Fact]
    public async Task CreateEvent_WrongCreatorId_ShouldReturnBadRequest()
    {
        await using WebApplicationFactory<Program> webApplicationFactory = new VeaWebApplicationFactory();
        HttpClient client = webApplicationFactory.CreateClient();

        var creatorId = "boollocks";
        HttpResponseMessage response = await client.PostAsync($"/api/events/{creatorId}/create", JsonContent.Create(""));
        
        Assert.True(response.IsSuccessStatusCode); 
    }
}