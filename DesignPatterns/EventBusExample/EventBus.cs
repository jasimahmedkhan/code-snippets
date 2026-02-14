namespace design_patterns.EventBusExample;

public class EventBus : IEventBus 
{
    private Dictionary<Type, List<Delegate>> _handlers = new();
    
    public void Subscribe<TEvent>(Action<TEvent> handler) 
    {
        var eventType = typeof(TEvent);
        if (!_handlers.ContainsKey(eventType)) 
        {
            _handlers[eventType] = new List<Delegate>();
        }
        _handlers[eventType].Add(handler);
    }
    
    public void Publish<TEvent>(TEvent @event) 
    {
        var eventType = typeof(TEvent);
        if (!_handlers.TryGetValue(eventType, out var handlers)) 
        {
            Console.WriteLine($"No handlers registered for event {eventType.Name}");
            return;
        }
        foreach (var handler in handlers) 
        {
            handler.DynamicInvoke(@event);
        }
    }
}


// Services (Colleagues) don't know about each other
public class InventoryService 
{
    private IEventBus _eventBus;
    
    public InventoryService(IEventBus eventBus) 
    {
        _eventBus = eventBus;
        _eventBus.Subscribe<OrderCreatedEvent>(OnOrderPlaced);
    }
    
    private void OnOrderPlaced(OrderCreatedEvent @event) 
    {
        Console.WriteLine($"Inventory: Reserving items for order {@event.OrderId}");
    }
}

public class EmailService 
{
    private IEventBus _eventBus;
    
    public EmailService(IEventBus eventBus) 
    {
        _eventBus = eventBus;
        _eventBus.Subscribe<OrderCreatedEvent>(OnOrderPlaced);
    }
    
    private void OnOrderPlaced(OrderCreatedEvent @event) 
    {
        Console.WriteLine($"Email: Sending confirmation for order {@event.OrderId}");
    }
}

public class AnalyticsService 
{
    private IEventBus _eventBus;
    
    public AnalyticsService(IEventBus eventBus) 
    {
        _eventBus = eventBus;
        _eventBus.Subscribe<OrderCreatedEvent>(OnOrderPlaced);
    }
    
    private void OnOrderPlaced(OrderCreatedEvent @event) 
    {
        Console.WriteLine($"Analytics: Recording sale of ${@event.Amount}");
    }
}

// Events
public class OrderCreatedEvent 
{
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
}

public class OrderCalculatedEvent
{
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
}

public class OrderPaymentReceivedEvent
{
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
}




// Usage - services completely decoupled
// Output:
// Inventory: Reserving items for order 123
// Email: Sending confirmation for order 123
// Analytics: Recording sale of $99.99

public class EventBusTests
{
    public static void TestEventBus() 
    {
        var eventBus = new EventBus();
        var inventoryService = new InventoryService(eventBus);
        var emailService = new EmailService(eventBus);
        var analyticsService = new AnalyticsService(eventBus);
        
        eventBus.Publish(new OrderCreatedEvent { OrderId = 123, Amount = 100 });
        
        eventBus.Publish(new OrderCalculatedEvent { OrderId = 123, Amount = 99.99m });
        
        eventBus.Publish(new OrderCreatedEvent { OrderId = 456, Amount = 200 });
        
        eventBus.Publish(new OrderPaymentReceivedEvent { OrderId = 123, Amount = 99.99m });
        
    }
}