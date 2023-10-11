namespace ReviseDotnet.Exceptions;

public class BadRequestException : Exception
{
    public int ErrorCode { get; set; }
    public readonly int Status = 400;

    public BadRequestException(string message, int errorCode = -1) : base(message)
    {
        this.ErrorCode = errorCode;
    }
}