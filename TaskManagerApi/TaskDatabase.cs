using TaskManagerApi.DataModels;

namespace TaskManagerApi;

public interface ITaskDatabase
{
    public Task<TaskItem?> GetTaskByName(string name, Guid userId);

    public Task<TaskItem[]> GetAllTasks(Guid ownerId);

    public  Task<TaskItem> InsertNewTask(TaskItemDto task, Guid userId);

    public Task DeleteTask(string name, Guid userId);

    public Task<TaskItem?> UpdateTask(string currentTaskName, TaskItemDto newTask, Guid userId);
}

public class TaskDatabase : ITaskDatabase
{
    public Task DeleteTask(string name, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<TaskItem[]> GetAllTasks(Guid ownerId)
    {
        throw new NotImplementedException();
    }

    public Task<TaskItem?> GetTaskByName(string name, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<TaskItem> InsertNewTask(TaskItemDto task, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<TaskItem?> UpdateTask(string currentTaskName, TaskItemDto newTask, Guid userId)
    {
        throw new NotImplementedException();
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
}