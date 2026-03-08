using PaymentProcessing.Result;
using PaymentProcessing.Services;

namespace PaymentProcessing.Stripe;

// ---- ADAPTER: StripeGateway ----
public class StripeAdapter : IPaymentGateway
{
    private readonly StripeAPI _stripeApi;

    public StripeAdapter(StripeAPI stripeApi)
    {
        _stripeApi = stripeApi;
    }

    public TransactionResult Charge(string customerId, decimal amount, string currency)
    {
        // Translates: decimal euros -> long cents, our model -> Stripe model
        var opts = new StripeChargeOptions()
        {
            Amount = (long)(amount * 100),
            Currency = currency,
            CustomerId = customerId
        };
        var charge = _stripeApi.CreateCharge(opts);
        return new TransactionResult { TransactionId = charge.Id, Success = true };
    }


    public bool Refund(string transactionId, decimal amount)
    {
        var refund = _stripeApi.CreateRefund(transactionId, (long)(amount * 100));
        return refund.Status == "succeeded";
    }
}