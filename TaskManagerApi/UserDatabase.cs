using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using TaskManagerApi.DataModels;

namespace TaskManagerApi;

public interface IUserDatabase
{
    public Task<User?> GetUser(string userName);

    public Task<User> InsertNewUser(LoginDetails loginDetails);

    public Task<User?> UpdateUser(User user);

    public Task DeleteUser(Guid id);
}

public class UserDatabase : IUserDatabase
{
    private SqlConnection dbConnection;

    public UserDatabase(SqlConnection connection)
    {
        dbConnection = connection;
    }

    public Task DeleteUser(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetUser(string userName)
    {
        return await dbConnection.QuerySingleOrDefaultAsync<User>("SELECT * FROM Users WHERE Name = @userName", new { userName});
    }

    public async Task<User> InsertNewUser(LoginDetails loginDetails)
    {
        var user = new User(Guid.NewGuid(), loginDetails.name, loginDetails.password);
        
        await dbConnection.ExecuteAsync("INSERT INTO Users(UserId, Name, Password) VALUES(@id, @name, @password);", user);

        return user;
    }

    public Task<User?> UpdateUser(User user)
    {
        throw new NotImplementedException();
    }
}

public class InMemoryUserDb : IUserDatabase
{
    private List<User> db = new List<User>();

    public void Empty()
    {
        db = new List<User>();
    }

    public async Task DeleteUser(Guid id)
    {
        await Task.CompletedTask;
        var user = db.Where(x => x.id == id).SingleOrDefault();

        if(user is null)
        {
            return;
        }

        db.Remove(user);

        return;
    }

    public async Task<User?> GetUser(string userName)
    {
        return await Task.FromResult(db
            .Where(x => x.name == userName)
            .SingleOrDefault());
    }

    public async Task<User> InsertNewUser(LoginDetails loginDetails)
    {
        var newUser = new User(Guid.NewGuid(), loginDetails.name, loginDetails.password);

        db.Add(newUser);

        return await Task.FromResult(newUser);
    }

    public async Task<User?> UpdateUser(User user)
    {
        await Task.CompletedTask;
        var existingTask = db.Where(x => x.id == user.id).SingleOrDefault();

        if(existingTask is null)
        {
            return null;
        }

        db.Remove(existingTask);

        db.Add(user);

        return user;
    }
}
