using PaymentProcessing.Result;
using PaymentProcessing.Services;

namespace PaymentProcessing.Mollie;

// --- ADAPTER 2 : MollieAdaptor
public class MollieAdapter : IPaymentGateway
{
    private readonly MollieClient _mollieClient;

    public MollieAdapter(MollieClient mollieClient)
    {
        _mollieClient = mollieClient;
    }

    public TransactionResult Charge(string customerId, decimal amount, string currency)
    {
        // Translates: decimal euros -> long cents, our model -> Mollie model
        var payment = _mollieClient.CreatePayment(customerId, amount, currency);
        return new TransactionResult { TransactionId = payment.Id, Success = true };
    }

    public bool Refund(string transactionId, decimal amount)
    {
        _mollieClient.CancelPayment(transactionId); // Mollie calls it "cancel"
        return true;
    }
}