namespace design_patterns.EventBusExample;

// Mediator as Event Bus
public interface IEventBus
{
    public void Subscribe<TEvent>(Action<TEvent> handler);
    public void Publish<TEvent>(TEvent @event);
}