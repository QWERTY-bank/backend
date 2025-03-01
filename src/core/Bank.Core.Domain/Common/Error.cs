namespace Bank.Core.Domain.Common;

public class Error
{
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }
    
    public string Code { get; init; }
    public string Message { get; init; }
}