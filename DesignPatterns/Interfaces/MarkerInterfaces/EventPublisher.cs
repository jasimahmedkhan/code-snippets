namespace design_patterns.Interfaces.MarkerInterfaces;

public interface IMediator
{
    Task Publish<T>(T @event) where T : IEvent;
}

public interface IMessageBus
{
    Task Publish<T>(T @event) where T : IEvent;
}

// Now your event bus can route differently
// This pattern lets you categorize events and handle them differently at runtime.
public class EventPublisher
{
    private readonly IMediator _mediator;
    private readonly IMessageBus _messageBus;

    public EventPublisher(IMediator mediator, IMessageBus messageBus)
    {
        _mediator = mediator;
        _messageBus = messageBus;
    }

    public async Task Publish<TEvent>(TEvent @event) where TEvent : IEvent
    {
        if (@event is IDomainEvent)
        {
            // Publish internally via MediatR
            await _mediator.Publish(@event);
        }
        else if (@event is IIntegrationEvent)
        {
            // Publish to external event bus RabbitMQ, Kafka, etc.
            await _messageBus.Publish(@event);
        }
    }
}
