namespace design_patterns.Interfaces.StrategyPattern;

public interface IPaymentStrategy
{
    Task<PaymentResult> ProcessPayment(decimal amount);
}

public class CreditCardPayment : IPaymentStrategy
{
    public Task<PaymentResult> ProcessPayment(decimal amount)
    {
        return Task.FromResult(new PaymentResult(true, "Payment successful via Credit Card"));
    }
}

public class PaypalPayment : IPaymentStrategy
{
    public Task<PaymentResult> ProcessPayment(decimal amount)
    {
        return Task.FromResult(new PaymentResult(true, "Payment successful via PayPal"));
    }
}

public class GooglePayPayment : IPaymentStrategy
{
    public Task<PaymentResult> ProcessPayment(decimal amount)
    {
        return Task.FromResult(new PaymentResult(true, "Payment successful via Google Pay"));
    }
}

public class DebitCardPayment : IPaymentStrategy
{
    public Task<PaymentResult> ProcessPayment(decimal amount)
    {
        return Task.FromResult(new PaymentResult(true, "Payment successful via Debit Card"));
    }
}

