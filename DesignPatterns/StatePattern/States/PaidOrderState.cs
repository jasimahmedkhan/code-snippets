namespace design_patterns.StatePattern.States;

public class PaidOrderState : IOrderState
{
    public string Name { get; }
    public void Pay(OrderContext context)
    {
        throw new InvalidOperationException("Order is already paid.");
    }

    public void Ship(OrderContext context)
    {
        Console.WriteLine("Shipping order...");
        context.ChangeState(new ShippedOrderState());
    }

    public void Deliver(OrderContext context)
    {
        throw new InvalidOperationException("Cannot deliver paid order without shipment");
    }

    public void Cancel(OrderContext context)
    {
        Console.WriteLine("Cancelling paid order... Refunding payment");
        context.ChangeState(new CancelledOrderState());
    }
}