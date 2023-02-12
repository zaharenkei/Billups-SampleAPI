namespace SampleAPI.Infrastructure.Exceptions;

public class ExternalClientException : CustomException
{
    public ExternalClientException(string message) : base(message)
    {

    }
}