﻿using Bank.Common.Application.Z1all.ExecutionResult.Interfaces;

namespace Bank.Common.Application.Z1all.ExecutionResult.Interfaces.StatusCode
{
    public interface IExecutionResult<TStatusEnum> : IExecutionResult
        where TStatusEnum : Enum
    {
        TStatusEnum StatusCode { get; }
        TStatusEnum DefaultStatusCode { get; }
    }

    public interface IExecutionResult<TSuccessResult, TStatusEnum> : Interfaces.IExecutionResult<TSuccessResult>, IExecutionResult<TStatusEnum>
        where TStatusEnum : Enum
    {
    }
}
