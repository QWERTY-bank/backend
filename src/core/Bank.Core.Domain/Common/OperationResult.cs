namespace Bank.Core.Domain.Common;

public class OperationResult
{
    public bool IsError { get; }
    public Error? Error { get; }
    
    public OperationResult()
    {
        this.Error = null;
        this.IsError = false;
    }
    
    public OperationResult(Error error)
    {
        this.Error = error;
        this.IsError = true;
    }
}