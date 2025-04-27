using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;
using Bank.Credits.Application.Requests.Configurations;
using Bank.Credits.Application.Requests.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;

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
            HttpClient httpClient,
            ILogger<CoreRequestService> logger)
        {
            _tokenService = tokenService;
            _options = options.Value;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ExecutionResult<BalanceDto>> GetAccountBalanceAsync(long accountId)
        {
            var getTokenResult = await _tokenService.GetServiceTokenAsync();
            if (getTokenResult.IsNotSuccess)
            {
                return ExecutionResult<BalanceDto>.FromError(getTokenResult);
            }

            var request = new HttpRequestMessage(HttpMethod.Get, SetPathId(_options.GetAccountBalance, accountId))
            {
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", getTokenResult.Result) }
            };

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error while sending request to {_options.GetAccountBalance}. StatusCode: {response.StatusCode}");
                return ExecutionResult<BalanceDto>.FromInternalServer("GetAccountBalance", $"Error while sending request to {_options.GetAccountBalance}. StatusCode: {response.StatusCode}");
            }

            var balance = await response.Content.ReadFromJsonAsync<BalanceDto>();
            if (balance == null)
            {
                _logger.LogError($"Error while deserializing response from {_options.GetAccountBalance}");
                return ExecutionResult<BalanceDto>.FromInternalServer("GetAccountBalance", $"Error while deserializing response from {_options.GetAccountBalance}");
            }

            return ExecutionResult<BalanceDto>.FromSuccess(balance);
        }

        private static string SetPathId(string path, long id)
        {
            return path.Replace("{id}", id.ToString());
        }
    }
}
