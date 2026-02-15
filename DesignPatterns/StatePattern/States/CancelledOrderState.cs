namespace design_patterns.StatePattern.States;

public class CancelledOrderState : IOrderState
{
    public string Name { get; }
    public void Pay(OrderContext context)
    {
        throw new InvalidOperationException("Order is already cancelled.");
    }

    public void Ship(OrderContext context)
    {
        throw new InvalidOperationException("Order is already cancelled.");
    }

    public void Deliver(OrderContext context)
    {
        throw new InvalidOperationException("Cannot deliver cancelled order.");
    }

    public void Cancel(OrderContext context)
    {
        throw new InvalidOperationException("Order is already cancelled.");
    }
}