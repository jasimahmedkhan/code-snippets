namespace PaymentProcessing.Result;

public class TransactionResult
{
    public string TransactionId { get; set; }
    public bool Success { get; set; }

    public override string ToString()
    {
        return $"TransactionId: {TransactionId}, Success: {Success}";
    }
}