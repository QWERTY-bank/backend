using Bank.Common.Application.Models;
using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;
using Bank.Credits.Application.Requests.Configurations;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Json;

namespace Bank.Credits.Application.Requests
{
    public class TokenService : ITokenService
    {
        private readonly TokenServiceOptions _options;
        private readonly IMemoryCache _cache;
        private readonly string _tokenKey = "ServiceToken";
        private readonly HttpClient _httpClient;
        private readonly ILogger<TokenService> _logger;

        public TokenService(
            IOptions<TokenServiceOptions> options, 
            IMemoryCache cache,
            HttpClient httpClient,
            ILogger<TokenService> logger)
        {
            _options = options.Value;
            _cache = cache;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ExecutionResult<string>> GetServiceTokenAsync()
        {
            if (TryGetTokenFromCache(out var token))
            {
                return ExecutionResult<string>.FromSuccess(token.Token);
            }

            ServiceTokenDto newToken = await GetTokenFromServiceAsync();

            SetTokenToCache(newToken);

            return ExecutionResult<string>.FromSuccess(newToken.Token);
        }

        private bool TryGetTokenFromCache([NotNullWhen(true)] out ServiceTokenDto? token)
        {
            if (!_cache.TryGetValue(_tokenKey, out token) || token == null)
            {
                return false;
            }

            if (token.ExpiredDateTime <= DateTime.UtcNow.AddSeconds(10))
            {
                return false;
            }

            return true;
        }

        private void SetTokenToCache(ServiceTokenDto token)
        {
            _cache.Set(_tokenKey, token, new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = token.ExpiredDateTime
            });
        }

        private async Task<ServiceTokenDto> GetTokenFromServiceAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _options.LoginPath)
            {
                Content = JsonContent.Create(new { secret = _options.Secret })
            };

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogCritical("Error while getting token from service. StatusCode: {StatusCode}", response.StatusCode);
                throw new HttpRequestException("Error while getting token from service", null, response.StatusCode);
            }

            var token = await response.Content.ReadFromJsonAsync<ServiceTokenDto>();
            if (token == null)
            {
                _logger.LogCritical("Error while getting token from service. Token is null");
                throw new HttpRequestException("Error while getting token from service", null, HttpStatusCode.InternalServerError);
            }

            return token;
        }
    }
}
