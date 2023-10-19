namespace TaskManagerApi.DataModels;



public record TaskOwner(Guid id, string name);


public record User(Guid id, string name, string password, string? email = null) : TaskOwner(id, name)
{
    private User() : this(default, default, default, default){}
}



public record Group(Guid id, string name, User[] members) : TaskOwner(id, name);