namespace design_patterns.StatePattern.States;

public class ShippedOrderState : IOrderState
{
    public string Name { get; }
    public void Pay(OrderContext context)
    {
        throw new InvalidOperationException("Cannot pay for shipped order.");
    }

    public void Ship(OrderContext context)
    {
        throw new InvalidOperationException("Order is already shipped.");
    }

    public void Deliver(OrderContext context)
    {
        Console.WriteLine("Delivering order...");
    }

    public void Cancel(OrderContext context)
    {
        throw new InvalidOperationException("Cannot cancel shipped order.");
    }
}