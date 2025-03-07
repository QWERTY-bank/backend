using Bank.Credits.Application.Requests.Configurations;
using Bank.Credits.Application.Requests.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Z1all.ExecutionResult.StatusCode;

namespace Bank.Credits.Application.Requests
{
    public class CoreRequestService : ICoreRequestService
    {
        private readonly ITokenService _tokenService;
        private readonly CoreRequestOptions _options;
        private readonly HttpClient _httpClient;
        private readonly ILogger<CoreRequestService> _logger;

        public CoreRequestService(
            ITokenService tokenService,
            IOptions<CoreRequestOptions> options,
            ILogger<CoreRequestService> logger)
        {
            _tokenService = tokenService;
            _options = options.Value;
            _httpClient = new()
            {
                BaseAddress = new Uri(_options.BaseUrl)
            };
            _logger = logger;
        }

        public async Task<ExecutionResult<BalanceDto>> GetAccountBalanceAsync(long accountId)
        {
            var getTokenResult = await _tokenService.GetServiceTokenAsync();
            if (getTokenResult.IsNotSuccess)
            {
                return ExecutionResult<BalanceDto>.FromError(getTokenResult.StatusCode, getTokenResult.Errors);
            }

            var request = new HttpRequestMessage(HttpMethod.Get, SetPathId(_options.GetAccountBalance, accountId))
            {
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", getTokenResult.Result) }
            };

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error while sending request to {_options.GetAccountBalance}. StatusCode: {response.StatusCode}");
                return ExecutionResult<BalanceDto>.FromError(StatusCodeExecutionResult.InternalServer, "GetAccountBalance", $"Error while sending request to {_options.GetAccountBalance}. StatusCode: {response.StatusCode}");
            }

            var balance = await response.Content.ReadFromJsonAsync<BalanceDto>();
            if (balance == null)
            {
                _logger.LogError($"Error while deserializing response from {_options.GetAccountBalance}");
                return ExecutionResult<BalanceDto>.FromError(StatusCodeExecutionResult.InternalServer, "GetAccountBalance", $"Error while deserializing response from {_options.GetAccountBalance}");
            }

            return ExecutionResult<BalanceDto>.FromSuccess(balance);
        }

        public Task<ExecutionResult> UnitAccountDepositTransferAsync(TransactionDto model, long accountId)
        {
            return Transfer(model, SetPathId(_options.UnitAccountDepositTransfer, accountId));
        }

        public Task<ExecutionResult> UnitAccountWithdrawTransferAsync(TransactionDto model, long accountId)
        {
            return Transfer(model, SetPathId(_options.UnitAccountWithdrawTransfer, accountId));
        }

        private async Task<ExecutionResult> Transfer(TransactionDto model, string path)
        {
            var getTokenResult = await _tokenService.GetServiceTokenAsync();
            if (getTokenResult.IsNotSuccess)
            {
                return ExecutionResult.FromError(getTokenResult);
            }

            var request = new HttpRequestMessage(HttpMethod.Post, path)
            {
                Content = JsonContent.Create(model),
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", getTokenResult.Result) }
            };

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error while sending request to {path}. StatusCode: {response.StatusCode}");
                return ExecutionResult.FromError(StatusCodeExecutionResult.InternalServer, "UnitAccountDepositTransfer", $"Error while sending request to {path}. StatusCode: {response.StatusCode}");
            }

            return ExecutionResult.FromSuccess();
        }

        private static string SetPathId(string path, long id)
        {
            return path.Replace("{id}", id.ToString());
        }
    }
}
