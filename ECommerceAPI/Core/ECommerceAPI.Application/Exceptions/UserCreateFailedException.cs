namespace ECommerceAPI.Application.Exceptions;

public class UserCreateFailedException : Exception
{
    public UserCreateFailedException() : base("User create failed.")
    {
        
    }

    public UserCreateFailedException(string? message) : base(message)
    
    {
        
    }

    // Creates a new Exception.  All derived classes should
    // provide this constructor.
    // Note: the stack trace is not started until the exception
    // is thrown
    
    public UserCreateFailedException(string? message, Exception? innerException) : base(message, innerException)
    
    {
        
    }
}