namespace design_patterns.StatePattern.States;

public class NewOrderState : IOrderState
{
    public string Name  => "New";
    
    public void Pay(OrderContext context)
    {
        Console.WriteLine("Charging payment for new order...");
        context.ChangeState(new PaidOrderState());
    }

    public void Ship(OrderContext context)
    {
        throw new InvalidOperationException("Cannot ship new order");
    }

    public void Deliver(OrderContext context)
    {
        throw new InvalidOperationException("Cannot deliver new order");
    }

    public void Cancel(OrderContext context)
    {
        Console.WriteLine("Cancelling new order...");
        context.ChangeState(new CancelledOrderState());
    }

}