using System.Security.Claims;
using TaskManagerApi.DataModels;

namespace TaskManagerApi;

public class TaskHandler
{
    private readonly ITaskDatabase taskDatabase;

    public TaskHandler(ITaskDatabase taskDatabase)
    {
        this.taskDatabase = taskDatabase;
    }

    public async Task<IResult> Get(string taskName, Guid userId)
    {
        
        var task = await taskDatabase.GetTaskByName(taskName, userId);

        if(task is null)
        {
            return Results.NotFound($"No task with name '{taskName}' exists for this user.");
        }

        return Results.Ok(task.ToDto());
    }

    public async Task<IResult> GetAll(Guid userId, string[] priorities, bool isComplete)
    {
        var tasks = await taskDatabase.GetAllTasks(userId);

        return Results.Ok(tasks
            .Where(x => x.isComplete == isComplete && priorities.Contains(x.priority.ToString()))
            .Select(x => x.ToDto()));
    }

    public async Task<IResult> Create(TaskItemDto task, Guid userId)
    {
        var existingTask = await taskDatabase.GetTaskByName(task.name, userId);
        if(existingTask is not null)
        {
            return Results.Conflict($"Could not create new task. There is already as task with name '{task.name}' assosiated with this user.");
        }

        var newTask = await taskDatabase.InsertNewTask(task, userId);

        return Results.Ok(newTask.ToDto());
    }

    public async Task<IResult> Update(string currentTaskName, TaskItemDto task, Guid userId)
    {
        var existingTaskWithNewName = await taskDatabase.GetTaskByName(task.name, userId);
        if(existingTaskWithNewName is not null && currentTaskName != task.name)
        {
            return Results.Conflict($"Could not update task name. There is already as task with name '{task.name}' assosiated with this user.");
        }

        var updatedTask = await taskDatabase.UpdateTask(currentTaskName, task, userId);

        if(updatedTask is null)
        {
            return Results.NotFound($"No task with the name '{currentTaskName}' is assosiated with this user.");
        }


        return Results.Ok(updatedTask!.ToDto());
    }

    public async Task<IResult> Delete(string taskName, Guid userId)
    {
        var existingTask = await taskDatabase.GetTaskByName(taskName, userId);
        if(existingTask is null)
        {
            return Results.NotFound($"Could not delete task. No task with the name '{taskName}' is assosiated with this user.");
        }

        await taskDatabase.DeleteTask(taskName, userId);

        return Results.Ok(existingTask.ToDto());
    }

    public async Task<IResult> Complete(string taskName, Guid userId)
    {
        var updatedTask = await taskDatabase.MarkComplete(taskName, userId);

        if(updatedTask is null)
        {
            return Results.NotFound($"No task with the name '{taskName}' is assosiated with this user.");
        }

        return Results.Ok(updatedTask!.ToDto());
    }
}