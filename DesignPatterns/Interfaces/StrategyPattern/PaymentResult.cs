namespace design_patterns.Interfaces.StrategyPattern;

public class PaymentResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    
    public PaymentResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }
    
    
}