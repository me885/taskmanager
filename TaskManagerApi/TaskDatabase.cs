using Dapper;
using Microsoft.Data.SqlClient;
using TaskManagerApi.DataModels;

namespace TaskManagerApi;

public interface ITaskDatabase
{
    public Task<TaskItem?> GetTaskByName(string name, Guid userId);

    public Task<TaskItem[]> GetAllTasks(Guid ownerId);

    public  Task<TaskItem> InsertNewTask(TaskItemDto task, Guid userId);

    public Task DeleteTask(string name, Guid userId);

    public Task<TaskItem?> UpdateTask(string currentTaskName, TaskItemDto newTask, Guid userId);

    public Task<TaskItem?> MarkComplete(string taskName, Guid userId);
}

public class TaskDatabase : ITaskDatabase
{
    private SqlConnection dbConnection;

    public TaskDatabase(SqlConnection connection)
    {
        dbConnection = connection;
    }
    
    public async Task DeleteTask(string name, Guid userId)
    {
        await dbConnection.ExecuteAsync(
            "DELETE FROM Tasks WHERE Name = @Name AND OwnerId = @OwnerId", 
            new { Name = name, OwnerId = userId}
        );
    }

    public async Task<TaskItem[]> GetAllTasks(Guid ownerId)
    {
        var result = await dbConnection.QueryAsync<TaskItem>("SELECT * FROM Tasks");

        return result.ToArray();
    }

    public async Task<TaskItem?> GetTaskByName(string name, Guid userId)
    {
        return await dbConnection.QuerySingleOrDefaultAsync<TaskItem>(
            "SELECT * FROM Tasks WHERE Name=@Name AND OwnerId=@UserId", 
            new { Name = name, UserId = userId}
        );
    }

    public async Task<TaskItem> InsertNewTask(TaskItemDto task, Guid userId)
    {
        var taskItem = new TaskItem(Guid.NewGuid(), task.name, task.description, userId, task.priority, task.deadline);

        await dbConnection.ExecuteAsync(
            "INSERT INTO Tasks(TaskId, Name, OwnerId, Description, Priority, Deadline, IsComplete) VALUES(@id, @name, @ownerId, @description, @priority, @deadline, @isComplete);",
            taskItem
        );

        return taskItem;
    }

    public async Task<TaskItem?> UpdateTask(string currentTaskName, TaskItemDto newTask, Guid userId)
    {
        var taskItem = await GetTaskByName(currentTaskName, userId);

        if(taskItem is null) return null;

        await dbConnection.ExecuteAsync(
            """
            Update Tasks 
            SET Name = @NewName, Description = @description, Priority = @priority, Deadline = @deadline IsComplete = @isComplete
            WHERE Name = @CurrentName AND OwnerId = @UserId
            """,
            new
            {
                CurrentName = currentTaskName,
                UserId = userId,
                NewName = newTask.name,
                newTask.description,
                newTask.priority,
                newTask.deadline,
                newTask.isComplete
            }
        );

        return taskItem;
    }

    public async Task<TaskItem?> MarkComplete(string taskName, Guid userId)
    {
        var taskItem = await GetTaskByName(taskName, userId);

        if(taskItem is null) return null;

        await dbConnection.ExecuteAsync(
            """
            Update Tasks 
            SET IsComplete = 1
            WHERE Name = @CurrentName AND OwnerId = @UserId
            """,
            new
            {
                CurrentName = taskName,
                UserId = userId
            }
        );

        return taskItem;
    }
}

public class InMemoryTaskDb : ITaskDatabase
{
    private List<TaskItem> db = new List<TaskItem>();

    public async Task<TaskItem?> GetTaskByName(string name, Guid userId)
    {
        return await Task.FromResult(
                db
                .Where(x => x.name == name && x.ownerId == userId)
                .SingleOrDefault()
            );
    }

    public async Task<TaskItem[]> GetAllTasks(Guid ownerId)
    {
        return await Task.FromResult(
                db.Where(x => x.ownerId == ownerId).ToArray()
            );
    }

    public async Task<TaskItem> InsertNewTask(TaskItemDto task, Guid userId)
    {
        var taskToCreate = new TaskItem(
            Guid.NewGuid(),
            task.name,
            task.description,
            userId,
            task.priority,
            task.deadline);
        
        db.Add(taskToCreate);

        return await Task.FromResult(taskToCreate);
    }

    public async Task DeleteTask(string name, Guid userId)
    {
        await Task.CompletedTask;
        var task = db.Where(x => x.name == name && x.ownerId == userId).SingleOrDefault();

        if(task is null)
        {
            return;
        }

        db.Remove(task);

        return;
    }

    public async Task<TaskItem?> UpdateTask(string currentTaskName, TaskItemDto newTask, Guid userId)
    {
        await Task.CompletedTask;
        var existingTaskIndex = db
            .FindIndex(x => x.name == currentTaskName && x.ownerId == userId);

        if(existingTaskIndex == -1)
        {
            return null;
        }

        db[existingTaskIndex] = db[existingTaskIndex] with 
        { 
            name = newTask.name,
            description = newTask.description,
            priority = newTask.priority,
            deadline = newTask.deadline
        };

        return await Task.FromResult(db[existingTaskIndex]);
    }

    public void Empty()
    {
        db = new List<TaskItem>();
    }

    public Task<TaskItem?> MarkComplete(string taskName, Guid userId)
    {
        throw new NotImplementedException();
    }
}