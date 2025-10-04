namespace MauiAuthPageTemplate.Exceptions;

public class FailureException : Exception
{
    public FailureException() : base("Failure.")
    {
    }
    public FailureException(string message) : base(message)
    {
    }
    public FailureException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
