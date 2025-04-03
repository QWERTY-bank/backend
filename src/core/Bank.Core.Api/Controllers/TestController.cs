using Bank.Common.Kafka;
using Bank.Common.Kafka.Transfers;
using Bank.Core.Application.Accounts.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Core.Api.Controllers;

[ApiController]
[Route("Test")]
public class TestController : ControllerBase
{
    private readonly ITopicProducer<UnitAccountTransferModel> _producer;

    public TestController(ITopicProducer<UnitAccountTransferModel> producer)
    {
        _producer = producer;
    }
    
    /// <summary>
    /// Добавляет/снимает со счета банка на счет пользователя (ручка для теста)
    /// </summary>
    [HttpPost]
    public async Task Test(UnitAccountTransferModel message)
    {
        await _producer.ProduceAsync(message.UserAccountId.ToString(), message, CancellationToken.None);
    }
}