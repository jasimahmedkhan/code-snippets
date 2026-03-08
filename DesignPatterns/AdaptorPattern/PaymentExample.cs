namespace design_patterns.AdaptorPattern;

// Target Interface
public interface IPaymentGateway
{
    TransactionResult Charge(string customerId, decimal amount, string currency);
    bool Refund(string transactionId, decimal amount);
}

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

public class StripeChargeOptions
{
    public string CustomerId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
}

public class StripeRefund
{
    public string Status { get; set; }
}

public class StripeCharge
{
    public string Id { get; set; }
}

public class TransactionResult
{
    public string TransactionId { get; set; }
    public bool Success { get; set; }

    public override string ToString()
    {
        return $"TransactionId: {TransactionId}, Success: {Success}";
    }
}

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

// ---- ADAPTEE 2: Mollie (popular in Germany/EU) ----
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

public class MollieRefund
{
}

public class MolliePayment
{
    public string Id { get; set; }
}

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

public class CheckoutService
{
    public readonly IPaymentGateway PaymentExample;

    public CheckoutService(IPaymentGateway paymentExample)
    {
        PaymentExample = paymentExample;
    }

    public bool ProcessPayment(string customerId, decimal amount)
    {
        var result = PaymentExample.Charge(customerId, amount, "EUR");
        return result.Success;
    }

    public bool Refund(string transactionId, decimal amount)
    {
        return PaymentExample.Refund(transactionId, amount);
    }
}

/*
 * Program.cs
 *
 * services.AddScoped<IPaymentGateway, StripeAdapter>();
 * services.AddScoped<IPaymentGateway, MollieAdapter>();
 * 
 */


