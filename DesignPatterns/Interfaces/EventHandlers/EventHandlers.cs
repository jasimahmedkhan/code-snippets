namespace design_patterns.Interfaces.EventHandlers;

public interface IEvent
{
    Guid EventId { get; }
    DateTime OccurredAt { get; }
}

public record OrderPlacedEvent(
    Guid EventId, DateTime OccurredAt, Guid OrderId, decimal TotalAmount, List<string> ProductIds) : IEvent;

// This is WHY your EventHandler uses a constraint!
public abstract class EventHandler<TEvent> 
    where TEvent : IEvent  // ← CONSTRAINT
{
    public async Task Handle(TEvent @event)
    {
        // Because of the constraint, you can access IEvent members:
        var eventId = @event.EventId;        // ✅ Works
        var timestamp = @event.OccurredAt;   // ✅ Works
        
        await ProcessEvent(@event);
    }
    
    protected abstract Task ProcessEvent(TEvent @event);
}

// Usage:
public class OrderHandler : EventHandler<OrderPlacedEvent> 
{
    // OrderPlacedEvent MUST implement IEvent, enforced at compile-time
    protected override Task ProcessEvent(OrderPlacedEvent @event)
    {
        
        return Task.CompletedTask;
    }
}



