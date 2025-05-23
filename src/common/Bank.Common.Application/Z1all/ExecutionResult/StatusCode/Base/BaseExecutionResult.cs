﻿using Bank.Common.Application.Z1all.ExecutionResult.Interfaces.StatusCode;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Bank.Common.Application.Z1all.ExecutionResult.StatusCode.Base
{
    public abstract class BaseExecutionResult<TStatusEnum> : IExecutionResult<TStatusEnum>
        where TStatusEnum : Enum
    {
        private ImmutableDictionary<string, List<string>> _errors = ImmutableDictionary<string, List<string>>.Empty;
        public ImmutableDictionary<string, List<string>> Errors
        {
            get { return _errors; }
            protected set
            {
                _errors = value;
                IsSuccess = false;
            }
        }

        private bool _isSuccess = false;
        public bool IsSuccess
        {
            get { return _isSuccess; }
            protected set
            {
                _isSuccess = value;
            }
        }
        public bool IsNotSuccess { get => !_isSuccess; }

        public TStatusEnum StatusCode { get; protected set; }
        public abstract TStatusEnum DefaultStatusCode { get; }

        public BaseExecutionResult()
        {
            IsSuccess = true;
            StatusCode = DefaultStatusCode;
        }
        public BaseExecutionResult(TStatusEnum status, string keyError, params string[] error)
        {
            IsSuccess = false;
            _errors = _errors.Add(keyError, error.ToList());
            StatusCode = status;
        }
        public BaseExecutionResult(TStatusEnum status, ImmutableDictionary<string, List<string>> errors)
        {
            IsSuccess = false;
            _errors = errors;
            StatusCode = status;
        }
    }

    public abstract class BaseExecutionResult<TSuccessResult, TStatusEnum> : BaseExecutionResult<TStatusEnum>, IExecutionResult<TSuccessResult, TStatusEnum>
        where TStatusEnum : Enum
    {
        private TSuccessResult? _result;
        public TSuccessResult Result
        {
            get
            {
                if (_result is null) throw new NullReferenceException();
                return _result;
            }
            protected set
            {
                _result = value;
                IsSuccess = true;
            }
        }

        public bool TryGetResult([NotNullWhen(true)] out TSuccessResult? result)
        {
            result = default;
            if (IsNotSuccess || Result is null) return false;

            result = Result;
            return true;
        }

        public BaseExecutionResult(TSuccessResult result) : base()
        {
            if (result is null) throw new ArgumentNullException();
            Result = result;
        }
        public BaseExecutionResult(TStatusEnum status, string keyError, params string[] error) : base(status, keyError, error) { }
        public BaseExecutionResult(TStatusEnum status, ImmutableDictionary<string, List<string>> errors) : base(status, errors) { }
    }
}
