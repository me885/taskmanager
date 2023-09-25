using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TaskManagerApi;
using TaskManagerApi.DataModels;

namespace TaskManagerTests.Tests;

public class TaskTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public TaskTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task CanCreateAndGetTaskSuccessfully()
    {
        var user = await CreateNewUserAndGetToken("jeff");
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {user!.token}");

        var taskToCreate = new TaskItemDto("walk_dog", "Take dog for a walk", TaskPriority.High);
        await _httpClient.PostAsJsonAsync("/task", taskToCreate);

        var retrievedTask = await _httpClient.GetFromJsonAsync<TaskItemDto>($"/task/{taskToCreate.name}");

        Assert.Equal(taskToCreate, retrievedTask);
    }

    [Fact]
    public async Task CanCreateAndDeleteTaskSuccessfully()
    {
        var user = await CreateNewUserAndGetToken("jeff");
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {user!.token}");

        var taskToCreate = new TaskItemDto("walk_dog", "Take dog for a walk", TaskPriority.High);
        await _httpClient.PostAsJsonAsync("/task", taskToCreate);

        var deleteResponse = await _httpClient.DeleteAsync($"/task/{taskToCreate.name}");


        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

        var getResponse = await _httpClient.GetAsync($"/task/{taskToCreate.name}");

        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task CanCreateAndEditTaskSuccessfully()
    {
        var user = await CreateNewUserAndGetToken("jeff");
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {user!.token}");

        var taskToCreate = new TaskItemDto("walk_dog", "Take dog for a walk", TaskPriority.High);
        await _httpClient.PostAsJsonAsync("/task", taskToCreate);

        var putResponse = await _httpClient.PutAsJsonAsync($"/task/{taskToCreate.name}", taskToCreate with {priority = TaskPriority.Low});

        var retrievedTask = await _httpClient.GetFromJsonAsync<TaskItemDto>($"/task/{taskToCreate.name}");

        Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
        Assert.Equal(taskToCreate with {priority = TaskPriority.Low}, retrievedTask);
    }

    [Fact]
    public async Task CreatingTwoTasksWithTheSameNameReturns409()
    {
        var user = await CreateNewUserAndGetToken("jeff");
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {user!.token}");

        var taskToCreate = new TaskItemDto("walk_dog", "Take dog for a walk", TaskPriority.High);
        await _httpClient.PostAsJsonAsync("/task", taskToCreate);

        var secondPostResponse = await _httpClient.PostAsJsonAsync("/task", taskToCreate);

        Assert.Equal(HttpStatusCode.Conflict, secondPostResponse.StatusCode);
    }

    [Fact]
    public async Task GettingTaskThatDoesNotExistReturns404()
    {
        var user = await CreateNewUserAndGetToken("jeff");
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {user!.token}");

        var getResponse = await _httpClient.GetAsync($"/task/walk_dog");

        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    private async Task<TokenResponse?> CreateNewUserAndGetToken(string name)
    {
        var loginDetails = new LoginDetails(name, "password123");
        await _httpClient.PostAsJsonAsync("/auth/register", loginDetails);
        var response = await _httpClient.PostAsJsonAsync("/auth/getToken", loginDetails);
        
        return await response.Content.ReadFromJsonAsync<TokenResponse>();
    }

    public void Dispose()
    {
        using var scope = _factory.Services.CreateScope();
        var userDb = scope.ServiceProvider.GetService<IUserDatabase>();
        var taskDb = scope.ServiceProvider.GetService<ITaskDatabase>();

        ((InMemoryUserDb)userDb!).Empty();
        ((InMemoryTaskDb)taskDb!).Empty();
    }
        
}