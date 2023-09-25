using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using TaskManagerApi;
using TaskManagerApi.DataModels;


namespace TaskManagerTests;

public class AuthTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public AuthTests(WebApplicationFactory<Program> factory)
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
    }

    [Fact]
    public async Task Register_Returns409_WhenDuplicateUsername()
    {
        var loginDetails = new LoginDetails("bob", "password123!");

        var response = await _httpClient.PostAsJsonAsync("/auth/register", loginDetails);
        var response2 = await _httpClient.PostAsJsonAsync("/auth/register", loginDetails);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, response2.StatusCode);
    }

    [Fact]
    public async Task GetToken_Returns401_WhenInvalidUsernameAndPassword()
    {
        var loginDetails = new LoginDetails("bob", "password123!");
        var incorrectLoginDetails = new LoginDetails("bob", "passwsaord123!");

        await _httpClient.PostAsJsonAsync("/auth/register", loginDetails);

        var response = await _httpClient.PostAsJsonAsync("/auth/getToken", incorrectLoginDetails);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetToken_Returns200_WhenValidUsernameAndPassword()
    {
        var loginDetails = new LoginDetails("bob", "password123!");

        await _httpClient.PostAsJsonAsync("/auth/register", loginDetails);

        var response = await _httpClient.PostAsJsonAsync("/auth/getToken", loginDetails);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetTasks_Returns401_WhenAuthHeaderIsInvaild()
    {
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer notValidToken");
        var response = await _httpClient.GetAsync("/Tasks");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetTasks_Returns200_WhenAuthHeaderIsVaild()
    {
        var loginDetails = new LoginDetails("bob", "password123!");

        await _httpClient.PostAsJsonAsync("/auth/register", loginDetails);

        var getTokenResponse = await _httpClient.PostAsJsonAsync("/auth/getToken", loginDetails);

        var token = $"Bearer {(await getTokenResponse.Content.ReadFromJsonAsync<TokenResponse>())!.token}";

        _httpClient.DefaultRequestHeaders.Add("Authorization", token);
        var response = await _httpClient.GetAsync("/tasks");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    public void Dispose()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var userDb = scope.ServiceProvider.GetService<IUserDatabase>();
            var taskDb = scope.ServiceProvider.GetService<ITaskDatabase>();
            
           ((InMemoryUserDb) userDb!).Empty();
           ((InMemoryTaskDb) taskDb!).Empty();
        }
    }
}