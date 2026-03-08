namespace PaymentProcessing.Stripe;

// --- ADAPTEE 1: Stripe ---
public class StripeAPI // Stripe API implementation -- SDK
{
    public StripeCharge CreateCharge(StripeChargeOptions options)
    {
        return new StripeCharge();
    }

    public StripeRefund CreateRefund(string chargeId, decimal amount)
    {
        return new StripeRefund();
    }
}