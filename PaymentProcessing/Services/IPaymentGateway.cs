using PaymentProcessing.Result;

namespace PaymentProcessing.Services;

public interface IPaymentGateway
{
    TransactionResult Charge(string customerId, decimal amount, string currency);
    bool Refund(string transactionId, decimal amount);
}