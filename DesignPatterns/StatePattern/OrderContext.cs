using design_patterns.StatePattern.States;

namespace design_patterns.StatePattern;

public class OrderContext
{
    private IOrderState State { get; set; }

    public OrderContext()
    {
        State = new NewOrderState();
    }


    public void ChangeState(IOrderState state)
    {
        State = state;
    }

    public string Status => State.GetType().Name;

    public void Pay() => State.Pay(this);
    public void Ship() => State.Ship(this);
    public void Deliver() => State.Deliver(this);
    public void Cancel() => State.Cancel(this);
}

public class OrderTests
{
    public static void TestOrder()
    {
        var order = new OrderContext();
        Console.WriteLine(order.Status); // New

        order.Pay();
        Console.WriteLine(order.Status); // Paid

        order.Ship();
        Console.WriteLine(order.Status); // Shipped 

        // order.Cancel(); // throws invalid operation
    }
}