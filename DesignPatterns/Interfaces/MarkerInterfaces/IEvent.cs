namespace design_patterns.Interfaces.MarkerInterfaces;

public interface IEvent
{
    public Guid EventId { get; }
    public DateTime OccurredAt { get; }
}

public interface IDomainEvent : IEvent { }

public interface IIntegrationEvent : IDomainEvent { }
