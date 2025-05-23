﻿using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Bank.Common.Application.Z1all.ExecutionResult.Interfaces
{
    public interface IExecutionResult
    {
        bool IsSuccess { get; }
        bool IsNotSuccess { get; }
        public ImmutableDictionary<string, List<string>> Errors { get; }
    }

    public interface IExecutionResult<TSuccessResult> : IExecutionResult
    {
        TSuccessResult Result { get; }
        bool TryGetResult([NotNullWhen(true)] out TSuccessResult? result);
    }
}
