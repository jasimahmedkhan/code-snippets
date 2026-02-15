namespace design_patterns.StatePattern;

public enum OrderStatus
{
    Placed, 
    Processing,
    PaymentReceived,
    Shipped,
    Delivered,
    Cancelled
}


public class Order
{
    private OrderStatus Status { get; set; }

    public void Pay()
    {
        if (Status == OrderStatus.Placed || Status == OrderStatus.Processing)
        {
            Status = OrderStatus.PaymentReceived;
        }
        else
        {
            throw new InvalidOperationException("Cannot pay in current state.");
        }
    }
    
    public void Ship()
    {
        if (Status == OrderStatus.PaymentReceived)
        {
            Status = OrderStatus.Shipped;
        }
        else
        {
            throw new InvalidOperationException("Cannot ship in current state.");
        }
    }
    
    public void Deliver()
    {
        if (Status == OrderStatus.Shipped)
        {
            Status = OrderStatus.Delivered;
        }
        else if (Status == OrderStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot deliver a cancelled order.");
        }
        else
        {
            throw new InvalidOperationException("Cannot deliver in current state.");
        }
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Placed || Status == OrderStatus.Processing)
        {
            Status = OrderStatus.Cancelled;
        }
        else
        {
            throw new InvalidOperationException("Cannot cancel order in current state.");
        }
    }
    
    
}