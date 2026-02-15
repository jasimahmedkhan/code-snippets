namespace design_patterns.StatePattern;

public interface IOrderState
{
    public string Name { get; }
    public void Pay(OrderContext context);
    public void Ship(OrderContext context);
    public void Deliver(OrderContext context);
    public void Cancel(OrderContext context);
}