namespace design_patterns.Interfaces.Security;

public interface IRequiresAdmin { }

public class DeleteUserCommand : IRequiresAdmin
{
    public Guid UserId { get; set; }
}

public class ViewUserCommand // Regular users can do this.
{
    public Guid UserId { get; set; }
}

// Middleware to enforce authorization based on interface markers
public class AuthorizationMiddleware
{
    private readonly IUser _user;
    
    public AuthorizationMiddleware(IUser user)
    {
        _user = user;
    }

    public async Task Handle<TCommand>(TCommand command) where TCommand : IRequiresAdmin
    {
        if (command is IRequiresAdmin && !_user.IsAdmin)
        {
            throw new UnauthorizedAccessException("Only admins can perform this action.");
        }
        await Task.CompletedTask;
    }
    
}


public interface IUser
{
    bool IsAdmin { get; }
}

public class User : IUser
{
    public bool IsAdmin { get; set; }
}

public class Admin : IUser
{
    public bool IsAdmin { get; } = true;
}   