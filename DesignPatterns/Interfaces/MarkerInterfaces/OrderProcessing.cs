namespace design_patterns.Interfaces.MarkerInterfaces;

// Marker interface - no methods, just a marker or a tag
public interface IOrderEvent { }


public class OrderPlacedEvent : IOrderEvent
{
    public Guid OrderId { get; set; }
}

public class UserRegisteredEvent : IOrderEvent
{
    public Guid UserId { get; set; }
}



public class OrderProcessing
{
    public void Process(Object obj)
    {
        if (obj is IOrderEvent)
        {
            Console.WriteLine("This is an event!");
            // Route to event bus
        }
        else
        {
            Console.WriteLine("This is not an event!");
        }
        
    }
}