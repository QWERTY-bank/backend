﻿using Bank.Common.Application.Models;
using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;

namespace Bank.Users.Application.Auth
{
    public interface IServiceTokenService
    {
        ExecutionResult<ServiceTokenDto> CreateServiceTokens();
    }
}
