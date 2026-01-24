namespace OrderProcessing.Events;

public record OrderPlacedEvent(
    Guid EventId, DateTime OccurredAt, Guid OrderId, decimal TotalAmount, List<string> ProductIds);