using System.Text.Json.Serialization;

namespace TaskManagerApi.DataModels;

public record TaskItem(
    Guid id,
    string name,
    string description,
    Guid ownerId,
    TaskPriority priority,
    DateTime? deadline = null);

public record TaskItemDto(
    string name,
    string description,
    TaskPriority priority,
    DateTime? deadline = null);


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TaskPriority
{
    Low, 
    Medium, 
    High
}


public static class TaskExtensions
{
    public static TaskItemDto ToDto(this TaskItem task)
    {
        return new TaskItemDto(task.name, task.description, task.priority, task.deadline);
    }
}