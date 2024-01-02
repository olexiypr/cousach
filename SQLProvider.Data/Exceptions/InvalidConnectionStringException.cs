namespace SQLProvider.Data.Exceptions;

public class InvalidConnectionStringException : Exception
{
    public InvalidConnectionStringException() : base("Unable to connect using provided connection string")
    {
        
    }
}