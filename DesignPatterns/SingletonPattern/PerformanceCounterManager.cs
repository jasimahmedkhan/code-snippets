namespace design_patterns.SingletonPattern;

// CORRECT: Immutable or properly synchronized
public sealed class PerformanceCounterManager
{
    private static readonly Lazy<PerformanceCounterManager> _instance =
        new Lazy<PerformanceCounterManager>(() => new PerformanceCounterManager());

    public static PerformanceCounterManager Instance => _instance.Value;

    private long _requestCount = 0;
    private long _errorCount = 0;

    private PerformanceCounterManager()
    {
    }

    public void IncrementRequestCount()
    {
        Interlocked.Increment(ref _requestCount); // Thread-safe
    }

    public void IncrementErrorCount()
    {
        Interlocked.Increment(ref _errorCount); // Thread-safe
    }

    public (long RequestCount, long ErrorCount) GetMetrics()
    {
        return (_requestCount, _errorCount);
    }
}

public static class PerformanceCounterManagerExtensions
{
    public static void LogMetrics(this PerformanceCounterManager manager)
    {
        var (requestCount, errorCount) = manager.GetMetrics();
        Console.WriteLine($"Request Count: {requestCount}, Error Count: {errorCount}");
    }
}

public class PerformanceCounterTest
{
    public static void PerformanceCounterTestMain()
    {
        var counter1 = PerformanceCounterManager.Instance;
        counter1.IncrementRequestCount();
        counter1.IncrementRequestCount();
        counter1.IncrementErrorCount();

        counter1.LogMetrics();


        var counter2 = PerformanceCounterManager.Instance;
        counter2.IncrementRequestCount();
        counter2.IncrementErrorCount();
        counter2.IncrementErrorCount();

        counter2.GetMetrics();
        counter2.LogMetrics();
    }
}