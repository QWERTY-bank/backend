﻿namespace Bank.Common.Application.Z1all.ExecutionResult.Errors
{
    public abstract record class BaseError(string KeyError, params string[] Error);
}
