namespace PaymentProcessing.Stripe;

public class StripeChargeOptions
{
    public string CustomerId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
}