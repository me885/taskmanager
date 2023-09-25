
namespace TaskManagerApi.DataModels;

public record LoginDetails(string name, string password);


public record TokenResponse(string token);