namespace HouseManagementApi.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException() { }

    public UserNotFoundException(string message) : base(message) { }

    public UserNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException() { }

    public UserAlreadyExistsException(string message) : base(message) { }

    public UserAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }
}

