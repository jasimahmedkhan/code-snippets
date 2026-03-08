namespace PaymentProcessing.Mollie;

public class MollieClient
{
    public MolliePayment CreatePayment(string customerId, decimal amount, string currency)
    {
        return new MolliePayment();
    }

    public MollieRefund CancelPayment(string paymentId)
    {
        return new MollieRefund();
    }
}