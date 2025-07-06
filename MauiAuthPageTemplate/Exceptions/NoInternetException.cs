namespace MauiAuthPageTemplate.Exceptions;

public class NoInternetException : Exception
{
    public NoInternetException() : base("No internet connection available.")
    {
    }
    public NoInternetException(string message) : base(message)
    {
    }
    public NoInternetException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
