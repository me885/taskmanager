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

    public async Task<IResult> GetAll(Guid userId)
    {
        var tasks = await taskDatabase.GetAllTasks(userId);

        return Results.Ok(tasks.Select(x => x.ToDto()));
    }

    public async Task<IResult> Create(TaskItemDto task, Guid userId)
    {
        var existingTask = await taskDatabase.GetTaskByName(task.name, userId);
        if(existingTask is not null)
        {
            return Results.Conflict($"Could not create new task. There is already as task with name '{task.name}' assosiated with this user.");
        }

        await taskDatabase.InsertNewTask(task, userId);

        return Results.Ok();
    }
}