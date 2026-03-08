using PaymentProcessing.Result;
using PaymentProcessing.Services;

namespace PaymentProcessing.PayPal;

public class PayPalAdapter : IPaymentGateway
{
    public TransactionResult Charge(string customerId, decimal amount, string currency)
    {
        throw new NotImplementedException();
    }

    public bool Refund(string transactionId, decimal amount)
    {
        throw new NotImplementedException();
    }
}