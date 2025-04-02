using Bank.Common.Kafka;
using Bank.Common.Kafka.Transfers;
using Bank.Credits.Domain.Credits;
using Bank.Credits.Persistence;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;

namespace Bank.Credits.Kafka
{
    public class TransferConsumer : ITopicConsumer<TransferResponse>
    {
        private readonly CreditsDbContext _dbContext;

        public TransferConsumer(CreditsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ConsumeAsync(ConsumeResult<string, TransferResponse> message, CancellationToken _)
        {
            var response = message.Message.Value;

            var payment = await _dbContext.Payments
                .Include(x => x.Credit!)
                    .ThenInclude(x => x.PaymentHistory.Where(x => x.PaymentStatus == PaymentStatusType.Canceled && x.Type == PaymentType.ReduceDebt))
                .Include(x => x.Credit!)
                    .ThenInclude(x => x.Tariff)
                .FirstOrDefaultAsync(x => x.Key == response.Key);

            if (payment == null || payment.PaymentStatus != PaymentStatusType.InProcess)
            {
                return;
            }

            Action<Payment, TransferStatus> action = payment.Type switch
            {
                PaymentType.ReduceDebt => ReduceDebt,
                PaymentType.Repayment => Repayment,
                PaymentType.IssuingCredit => IssuingCredit,
                _ => throw new NotImplementedException(),
            };

            action(payment, response.Status);

            await _dbContext.SaveChangesAsync();
        }

        private void ReduceDebt(Payment payment, TransferStatus status)
        {
            if (status == TransferStatus.Success)
            {
                payment.PaymentStatus = PaymentStatusType.Conducted;
                payment.Credit!.UpdateCreditStatus();
            }
            else
            {
                payment.PaymentStatus = PaymentStatusType.Canceled;
                payment.Credit!.CancelPayment(payment.PaymentAmount);
            }
        } 
        
        private void Repayment(Payment payment, TransferStatus status)
        {
            if (status == TransferStatus.Success)
            {
                payment.PaymentStatus = PaymentStatusType.Conducted;
                payment.Credit!.UpdateCreditStatus();
            }
            else
            {
                payment.PaymentStatus = PaymentStatusType.Overdue;
                payment.Credit!.CancelPayment(payment.PaymentAmount);
            }
        }        
        
        private void IssuingCredit(Payment payment, TransferStatus status)
        {

            if (status == TransferStatus.Success)
            {
                payment.PaymentStatus = PaymentStatusType.Conducted;
                payment.Credit!.TakeCredit();
            }
            else
            {
                payment.PaymentStatus = PaymentStatusType.Canceled;
                payment.Credit!.CancelCredit();
            }
        }
    }
}
