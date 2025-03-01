namespace Bank.Core.Domain.Common;

public static class OperationResultFactory
{
    public static readonly OperationResult<Empty> EmptyResult = new(Empty.Instance);

    public static OperationResult<T> Success<T>(T success) => new(success);

    public static OperationResult<T> FromError<T>(Error error) => new(error);

    public static OperationResult<T> NameIsNotUnique<T>(string title, string objectName) =>
        new OperationResult<T>(
            new Error(
                OperationErrorCodes.InvalidData,
                $"{objectName} c названием {title} уже существует"));
    
    public static OperationResult<T> NotFound<T>(long id) =>
        new OperationResult<T>(
            new Error(
                OperationErrorCodes.NotFound,
                $"Сущность с идентификатором {id} не найдена"));
}