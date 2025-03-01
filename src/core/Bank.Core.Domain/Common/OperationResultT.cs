namespace Bank.Core.Domain.Common;

public class OperationResult<TValue> : OperationResult
{
    public TValue? Value { get; }
    
    public OperationResult(Error error) : base(error)
    {
        Value = default;
    }
    
    public OperationResult(TValue result) => Value = result;
}