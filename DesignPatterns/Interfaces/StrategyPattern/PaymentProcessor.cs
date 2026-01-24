namespace design_patterns.Interfaces.StrategyPattern;

public class PaymentProcessor
{
    private IPaymentStrategy _paymentStrategy;
    
    public PaymentProcessor(IPaymentStrategy paymentStrategy)
    {
        _paymentStrategy = paymentStrategy;
    }

    public async Task<PaymentResult> ProcessPayment(decimal amount)
    {
        return await _paymentStrategy.ProcessPayment(amount);
    }
}