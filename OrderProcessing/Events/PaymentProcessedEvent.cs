namespace OrderProcessing.Events;

public record PaymentProcessedEvent(
    Guid EventId, DateTime OccurredAt, Guid OrderId, decimal Amount, string PaymentMethod);