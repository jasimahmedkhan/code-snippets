namespace OrderProcessing.Events;

public record InventoryReservedEvent(Guid EventId, DateTime OccurredAt, List<string> ProductIds) : IEvent;