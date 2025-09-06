namespace HouseManagementApi.Exceptions;

public class HouseNotFoundException : Exception
{
    public HouseNotFoundException() { }

    public HouseNotFoundException(string message) : base(message) { }

    public HouseNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}

