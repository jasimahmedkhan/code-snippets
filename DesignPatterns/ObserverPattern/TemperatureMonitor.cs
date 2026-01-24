namespace design_patterns.ObserverPattern;

// OBSERVER - The thing watching for changes
// Observer 1 - Display
public class TemperatureDisplay : IObserver<int> 
{
    private string _name;
    private IDisposable _unsubscriber;
    
    public TemperatureDisplay(string name)
    {
        _name = name;
    }
    
    // Called when sensor has new temperature data
    public void OnNext(int temp) 
    {
        Console.WriteLine($"[{_name}] Current temperature: {temp}°C");
    }
    
    // Called when sensor encounters an error
    public void OnError(Exception error) 
    {
        Console.WriteLine($"[{_name}]  Error: {error.Message}");
    }
    
    // Called when sensor stops producing data
    public void OnCompleted() 
    {
        Console.WriteLine($"[{_name}] ✓ Temperature monitoring completed");
    }
    
    // Subscribe to a sensor
    public void Subscribe(IObservable<int> sensor) 
    {
        _unsubscriber = sensor.Subscribe(this);
        Console.WriteLine($"[{_name}] Subscribed to sensor");
    }
    
    // Unsubscribe from sensor
    public void Unsubscribe() 
    {
        _unsubscriber.Dispose();
        Console.WriteLine($"[{_name}] Unsubscribed from sensor");
    }
}

// Observer 2 - Alert system
public class TemperatureAlert : IObserver<int>
{
    private int _threshold;
    private IDisposable _unsubscriber;
    
    public TemperatureAlert(int threshold)
    {
        _threshold = threshold;
    }
    
    public void OnNext(int temp)
    {
        if (temp > _threshold)
        {
            Console.WriteLine($"[ALERT] Temperature {temp}°C exceeds threshold of {_threshold}°C!");
        }
    }
    
    public void OnError(Exception error)
    {
        Console.WriteLine($"[ALERT] Error in monitoring: {error.Message}");
    }
    
    public void OnCompleted()
    {
        Console.WriteLine("[ALERT] Alert system shutting down");
    }
    
    public void Subscribe(IObservable<int> sensor)
    {
        _unsubscriber = sensor.Subscribe(this);
    }
    
    public void Unsubscribe()
    {
        _unsubscriber.Dispose();
    }
}