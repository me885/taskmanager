using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using TaskManagerApi;
using TaskManagerApi.DataModels;


namespace TaskManagerTests;

public class TaskTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public TaskTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
    }


    [Fact]
    public async Task Register_Returns200_WhenValidBody()
    {
        var loginDetails = new LoginDetails("bob", "password123!");

        var response = await _httpClient.PostAsJsonAsync("/auth/register", loginDetails);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        CleanupFakeDb();
    }

    [Fact]
    public async Task Register_Returns409_WhenDuplicateUsername()
    {
        var loginDetails = new LoginDetails("bob", "password123!");

        var response = await _httpClient.PostAsJsonAsync("/auth/register", loginDetails);
        var response2 = await _httpClient.PostAsJsonAsync("/auth/register", loginDetails);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, response2.StatusCode);

        CleanupFakeDb();
    }

    [Fact]
    public async Task GetToken_Returns401_WhenInvalidUsernameAndPassword()
    {
        var loginDetails = new LoginDetails("bob", "password123!");
        var incorrectLoginDetails = new LoginDetails("bob", "passwsaord123!");

        await _httpClient.PostAsJsonAsync("/auth/register", loginDetails);

        var response = await _httpClient.PostAsJsonAsync("/auth/getToken", incorrectLoginDetails);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        CleanupFakeDb();
    }

    [Fact]
    public async Task GetToken_Returns200_WhenValidUsernameAndPassword()
    {
        var loginDetails = new LoginDetails("bob", "password123!");

        await _httpClient.PostAsJsonAsync("/auth/register", loginDetails);

        var response = await _httpClient.PostAsJsonAsync("/auth/getToken", loginDetails);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        CleanupFakeDb();
    }

    private void CleanupFakeDb()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var userDb = scope.ServiceProvider.GetService<IUserDatabase>();
            
           ((InMemoryUserDb) userDb!).Empty();
        }
    }
}