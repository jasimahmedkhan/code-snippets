namespace design_patterns.ObserverPattern;

// SUBJECT/PRODUCER - The thing being observed
public class TemperatureSensor : IObservable<int> 
{
    private List<IObserver<int>> _observers = new();
    private int _currentTemperature;
    
    public IDisposable Subscribe(IObserver<int> observer) 
    {
        if (observer == null)
            throw new ArgumentNullException(nameof(observer));
    
        lock (_observers)  // Add thread safety
        {
            if (!_observers.Contains(observer))  // Invert the condition for better readability
            {
                _observers.Add(observer);
                observer.OnNext(_currentTemperature);  // Notify of current temperature
            }
            return new Unsubscriber(_observers, observer);
        }
    }
    
    public void SetTemperature(int temp) 
    {
        _currentTemperature = temp;
        
        // Notify all observers about the new temperature
        foreach (var observer in _observers) 
        {
            observer.OnNext(temp);
        }
    }
    
    // Simulate a sensor error
    public void ReportError(string errorMessage)
    {
        var exception = new Exception(errorMessage);
        foreach (var observer in _observers)
        {
            observer.OnError(exception);
        }
    }
    
    // Signal that sensor is shutting down
    public void StopMonitoring()
    {
        foreach (var observer in _observers)
        {
            observer.OnCompleted();
        }
        _observers.Clear();
    }
    
    // Helper class to handle unsubscription
    private class Unsubscriber : IDisposable 
    {
        private List<IObserver<int>> _observers;
        private IObserver<int> _observer;
        
        public Unsubscriber(List<IObserver<int>> observers, IObserver<int> observer) 
        {
            _observers = observers;
            _observer = observer;
        }
        
        public void Dispose() 
        {
            if (_observer != null && _observers.Contains(_observer))
            {
                _observers.Remove(_observer);
            }
        }
    }
}
