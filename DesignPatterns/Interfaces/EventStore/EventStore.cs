namespace design_patterns.Interfaces.EventStore;

public class EventStore
{
    // Multiple interface constraints
    public class EventStore<T> 
        where T : IEvent, ISerializable, IValidatable
    {
        public async Task StoreAsync(T @event)
        {
            @event.Validate();           // From IValidatable
            var json = @event.Serialize(); // From ISerializable
            var id = @event.EventId;       // From IEvent
            
            await _storage.SaveAsync(id, json);
        }
    }
}