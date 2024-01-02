namespace SQLProvider.Data.Exceptions;

public class ConnectionNotFoundException : Exception
{
    public ConnectionNotFoundException(int connectionId) : base($"Connection with id {connectionId} is not found") 
    {
        
    }
}