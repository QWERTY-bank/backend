using System.ComponentModel.DataAnnotations;

namespace Bank.Core.Api.Hubs.Models;

public class SubscribeToAccountModel
{
    [Required]
    public long AccountId { get; init; }
}