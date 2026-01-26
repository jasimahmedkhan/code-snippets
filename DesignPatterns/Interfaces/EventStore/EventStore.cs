namespace design_patterns.Interfaces.EventStore;

public interface IEvent
{
    Guid EventId { get; }
}

public interface ISerializable
{
    string Serialize();
}

public interface IValidatable
{
    void Validate();
}

public interface IDataStorage
{
    Task SaveAsync(Guid id, string json);
}

// Multiple interface constraints
public class EventStore<T>
    where T : IEvent, ISerializable, IValidatable
{
    private IDataStorage _storage;

    public EventStore(IDataStorage storage)
    {
        _storage = storage;
    }

    public async Task StoreAsync(T @event)
    {
        @event.Validate(); // From IValidatable
        var json = @event.Serialize(); // From ISerializable
        var id = @event.EventId; // From IEvent

        await _storage.SaveAsync(id, json);
    }
}

public class OrderPlacedEvent: IEvent, ISerializable, IValidatable
{
    public Guid EventId { get; }
    public string Serialize()
    {
        Console.WriteLine("Serializing order");
        return string.Empty;
    }

    public void Validate()
    {
        Console.WriteLine("Validating order");
    }
}

public class PostgresDataStorage : IDataStorage
{
    public Task SaveAsync(Guid id, string json)
    {
        return Task.CompletedTask;
    }
}

public class EventStoreDemo
{
    public async Task RunAsync()
    {
        var postgresDataStorage = new PostgresDataStorage();
        var eventStore = new EventStore<OrderPlacedEvent>(postgresDataStorage);
        await eventStore.StoreAsync(new OrderPlacedEvent());
    }
}

    

