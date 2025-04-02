using Bank.Credits.Domain.Credits;
using System.Text.Json.Serialization;

namespace Bank.Credits.Application.Requests.Models
{
    public class CurrencyValueDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required CurrencyCode Code { get; init; }
        public required decimal Value { get; init; }
    }
}
