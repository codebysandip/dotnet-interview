namespace ReviseDotnet.Exceptions;

public class InternalServerException : Exception
{
    public int ErrorCode { get; set; }
    public readonly int Status = 500;

    public InternalServerException(string message, int errorCode = -1) : base(message)
    {
        this.ErrorCode = errorCode;
    }
}