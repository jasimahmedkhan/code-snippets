namespace design_patterns.SingletonPattern;

// BEST APPROACH: Thread-safe, lazy, simple, performs well
public sealed class LazySingleton
{
    // Lazy<T> handles thread-safety and lazy initialization
    private static readonly Lazy<LazySingleton> _lazy = new Lazy<LazySingleton>(() => new LazySingleton());

    private LazySingleton()
    {
        Console.WriteLine("Singleton initialized lazily");
    }

    public static LazySingleton Instance => _lazy.Value;

    // Can check if instance created yet
    public static bool IsCreated => _lazy.IsValueCreated;
}

// Pros: Simple, thread-safe automatically
// Cons: Instance created even if never used (wastes memory)
public sealed class EagerSingleton
{
    // Instance created at class loading time - guaranteed thread-safe by CLR
    private static readonly EagerSingleton _instance = new EagerSingleton();

    // Explicit static constructor tells C# compiler not to mark as beforefieldinit
    static EagerSingleton()
    {
    }

    private EagerSingleton()
    {
        Console.WriteLine("Singleton initialized eagerly");
    }

    public static EagerSingleton Instance => _instance;
}

// Only locks on first creation - much better performance
public sealed class DoubleCheckSingleton
{
    private static DoubleCheckSingleton _instance = null;
    private static readonly object _lock = new object();

    private DoubleCheckSingleton()
    {
    }

    public static DoubleCheckSingleton Instance
    {
        get
        {
            // First check (no lock) - fast path for existing instance
            if (_instance == null)
            {
                lock (_lock)
                {
                    // Second check (with lock) - ensure thread safety
                    if (_instance == null)
                    {
                        _instance = new DoubleCheckSingleton();
                    }
                }
            }

            return _instance;
        }
    }
}

public sealed class ThreadSingleton
{
    private static ThreadSingleton _instance = null;
    private static readonly object _lock = new object();

    private ThreadSingleton()
    {
    }

    public static ThreadSingleton Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new ThreadSingleton();
                }

                return _instance;
            }
        }
    }
}

public class ProgramThreadSafeSingleton
{
    public static void ThreadSafeSingletonTest()
    {
        var instance1 = ThreadSingleton.Instance;
        var instance2 = ThreadSingleton.Instance;

        Console.WriteLine(ReferenceEquals(instance1, instance2));

        Console.WriteLine($"Instance 1: {instance1}, Instance 2: {instance2}");
    }

    public static void DoubleCheckSingletonTest()
    {
        var instance1 = DoubleCheckSingleton.Instance;
        var instance2 = DoubleCheckSingleton.Instance;

        Console.WriteLine(ReferenceEquals(instance1, instance2));

        Console.WriteLine($"Instance 1: {instance1}, Instance 2: {instance2}");
    }

    public static void EagerSingletonTest()
    {
        var instance1 = EagerSingleton.Instance;
        var instance2 = EagerSingleton.Instance;

        Console.WriteLine(ReferenceEquals(instance1, instance2));

        Console.WriteLine($"Instance 1: {instance1}, Instance 2: {instance2}");
    }

    public static void LazySingletonTest()
    {
        var instance1 = LazySingleton.Instance;
        var instance2 = LazySingleton.Instance;
        Console.WriteLine(ReferenceEquals(instance1, instance2));
        Console.WriteLine($"Instance 1: {instance1}, Instance 2: {instance2}");
        Console.WriteLine(LazySingleton.IsCreated);
        
    }
    
    public static void MainTest()
    {
        ThreadSafeSingletonTest();
        DoubleCheckSingletonTest();
        EagerSingletonTest();
        LazySingletonTest();
    }
    
}