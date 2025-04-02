using Bank.Common.Kafka;
using Bank.Common.Kafka.Transfers;
using Bank.Core.Application;
using Bank.Core.Application.Accounts.Models;
using Bank.Core.Application.Accounts.Unit;
using Bank.Core.Domain.Common;
using Bank.Core.Domain.Currencies;
using Confluent.Kafka;
using MediatR;

namespace Bank.Core.Kafka.Transfers;

public class TransferConsumer : ITopicConsumer<UnitAccountTransferModel>
{
    private readonly IMediator _mediator;
    private readonly ITopicProducer<TransferResponse> _producer;
    
    public TransferConsumer(
        ICoreDbContext dbContext, 
        IMediator mediator, 
        ITopicProducer<TransferResponse> producer)
    {
        _mediator = mediator;
        _producer = producer;
    }

    public async Task ConsumeAsync(ConsumeResult<string, UnitAccountTransferModel> message, CancellationToken cancellationToken)
    {
        var value = message.Message.Value;

        try
        {
            OperationResult<Empty> result;
            
            if (value.Type == UnitTransferType.Deposit)
            {
                var command = new UnitAccountDepositTransferCommand
                {
                    Key = value.Key,
                    UnitId = value.UnitId,
                    UserAccountId = value.UserAccountId,
                    CurrencyValue = new CurrencyValue
                    {
                        Code = value.CurrencyValue.Code switch
                        {
                            TransferCurrencyCode.Rub => CurrencyCode.Rub,
                            TransferCurrencyCode.Usd => CurrencyCode.Usd,
                            TransferCurrencyCode.Eur => CurrencyCode.Eur,
                            _ => throw new ArgumentOutOfRangeException()
                        },
                        Value = value.CurrencyValue.Value
                    }
                };
                
                result = await _mediator.Send(command, cancellationToken);
            }
            else
            {
                var command = new UnitAccountWithdrawTransferCommand()
                {
                    Key = value.Key,
                    UnitId = value.UnitId,
                    UserAccountId = value.UserAccountId,
                    CurrencyValue = new CurrencyValue
                    {
                        Code = value.CurrencyValue.Code switch
                        {
                            TransferCurrencyCode.Rub => CurrencyCode.Rub,
                            TransferCurrencyCode.Usd => CurrencyCode.Usd,
                            TransferCurrencyCode.Eur => CurrencyCode.Eur,
                            _ => throw new ArgumentOutOfRangeException()
                        },
                        Value = value.CurrencyValue.Value
                    }
                };
                
                result = await _mediator.Send(command, cancellationToken);
            }

            var transferResponse = new TransferResponse
            {
                Key = value.Key,
                Status = result.IsError ? TransferStatus.Fail : TransferStatus.Success
            };

            await _producer.ProduceAsync(
                transferResponse.Key.ToString(), 
                transferResponse, 
                cancellationToken);
        }
        catch (Exception e)
        {
            var transferResponse = new TransferResponse
            {
                Key = value.Key,
                Status = TransferStatus.Fail
            };
            
            await _producer.ProduceAsync(
                transferResponse.Key.ToString(), 
                transferResponse, 
                cancellationToken);
        }
    }
}