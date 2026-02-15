namespace program_snippets.Delegates;

public class LRUCache<TKey, TValue>
{
    private readonly int _capacity;
    private readonly Dictionary<TKey, LinkedListNode<CacheItem>> _cache;
    private readonly LinkedList<CacheItem> _lruList;

    private class CacheItem
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
    }

    public LRUCache(int capacity)
    {
        _capacity = capacity;
        _cache = new Dictionary<TKey, LinkedListNode<CacheItem>>();
        _lruList = new LinkedList<CacheItem>();
    }

    public bool TryGet(TKey key, out TValue value)
    {
        if (_cache.TryGetValue(key, out var node))
        {
            // Move to front (most recently used)
            _lruList.Remove(node);
            _lruList.AddFirst(node);

            value = node.Value.Value;
            return true;
        }

        value = default(TValue);
        return false;
    }

    public void Add(TKey key, TValue value)
    {
        if (_cache.Count >= _capacity)
        {
            // Remove least recently used (at the end)
            var lastNode = _lruList.Last;
            _cache.Remove(lastNode.Value.Key);
            _lruList.RemoveLast();
        }

        // Add to front (most recently used)
        var newItem = new CacheItem { Key = key, Value = value };
        var newNode = new LinkedListNode<CacheItem>(newItem);
        _lruList.AddFirst(newNode);
        _cache[key] = newNode;
    }

    // Memoization using LRU Cache
    public static Func<T, TResult> MemoizeLRU<T, TResult>(
        Func<T, TResult> func,
        int maxCacheSize = 100)
    {
        var cache = new LRUCache<T, TResult>(maxCacheSize);

        return input =>
        {
            if (cache.TryGet(input, out var result))
            {
                Console.WriteLine($"Cache hit for {input}");
                return result;
            }

            Console.WriteLine($"Computing for {input}");
            result = func(input);
            cache.Add(input, result);

            return result;
        };
    }
}

public class Memoization
{
    //  We're creating a wrapper function with the same signature
    //  The original function we want to cache
    public static Func<T, TResult> Memoize<T, TResult>(Func<T, TResult> func)
    {
        //  Create a private cache for THIS memoized function
        //  ⚠️ This cache is NOT shared between different memoized functions
        var cache = new Dictionary<T, TResult>();

        return input =>
            //  Create and return a NEW function
            //  This new function takes the same input as the original
        {
            //  Check: "Have I calculated this input before?"
            if (cache.ContainsKey(input))
            {
                // YES: Return the saved result (FAST!)
                return cache[input];
            }

            //   NO: Call the original function (SLOW)
            var result = func(input);

            //  Save the result so next time we can skip the calculation
            cache[input] = result;

            //  Return the result we just calculated
            return result;
        };
    }

    public static Func<T, TResult> MemoizeWithLimit<T, TResult>(
        Func<T, TResult> func,
        int maxCacheSize = 100)
    {
        // Store values
        var cache = new Dictionary<T, TResult>();

        // Store access timestamps
        var accessTimes = new Dictionary<T, DateTime>();

        return input =>
        {
            // Cache hit - update timestamp
            if (cache.ContainsKey(input))
            {
                accessTimes[input] = DateTime.Now; // Update last access time
                Console.WriteLine($"Cache hit for {input}");
                return cache[input];
            }

            // Cache miss - compute
            Console.WriteLine($"Computing for {input}");
            var result = func(input);

            // Check if cache is full
            if (cache.Count >= maxCacheSize)
            {
                // Find the least recently used item
                var oldestKey = accessTimes
                    .OrderBy(kvp => kvp.Value) // Sort by timestamp
                    .First() // Get the oldest
                    .Key;

                // Remove it
                cache.Remove(oldestKey);
                accessTimes.Remove(oldestKey);
                Console.WriteLine($"Cache full - evicted {oldestKey}");
            }

            // Add to cache
            cache[input] = result;
            accessTimes[input] = DateTime.Now;

            return result;
        };
    }
}

public class MemoizationTests
{
    public static void TestMemoization()
    {
        Func<int, int> expensiveFunc = n =>
        {
            Console.WriteLine("Expensive calculation: ");
            Thread.Sleep(1000); // simulate expensive calculation (1 second)
            return n * n;
        };

        Func<int, int> memoizedSquare = Memoization.Memoize(expensiveFunc);

        Console.WriteLine("=== First call with 5 ===");
        int result1 = memoizedSquare(5);
        // Output:
        // Computing for 5
        //   [EXPENSIVE] Computing 5 * 5...
        // (waits 1 second)
        Console.WriteLine($"Result: {result1}"); // 25

        Console.WriteLine("\n=== Second call with 5 ===");
        int result2 = memoizedSquare(5);
        // Output:
        // Cache hit for 5
        // (instant - no wait!)
        Console.WriteLine($"Result: {result2}"); // 25

        Console.WriteLine("\n=== First call with 10 ===");
        int result3 = memoizedSquare(10);
        // Output:
        // Computing for 10
        //   [EXPENSIVE] Computing 10 * 10...
        // (waits 1 second)
        Console.WriteLine($"Result: {result3}"); // 100

        Console.WriteLine("\n=== Second call with 5 again ===");
        int result4 = memoizedSquare(5);
        // Output:
        // Cache hit for 5
        // (instant!)
        Console.WriteLine($"Result: {result4}"); // 25       
    }

    public static void TestRealWorldUseCase()
    {
        Func<string, string> fetchUserData = userId =>
        {
            Console.WriteLine($"  [API] Fetching data for user {userId}...");
            Thread.Sleep(2000); // Simulate network delay
            return $"{{name: 'User {userId}', email: 'user{userId}@example.com'}}";
        };

        // Wrap with memoization
        Func<string, string> cachedFetchUserData = Memoization.Memoize(fetchUserData);

        // First request - hits API
        Console.WriteLine("Request 1:");
        string data1 = cachedFetchUserData("123");
        Console.WriteLine(data1);
        // Computing for 123
        //   [API] Fetching data for user 123...
        // (waits 2 seconds)
        // {name: 'User 123', email: 'user123@example.com'}

        // Second request - uses cache
        Console.WriteLine("\nRequest 2:");
        string data2 = cachedFetchUserData("123");
        Console.WriteLine(data2);
        // Cache hit for 123
        // (instant!)
        // {name: 'User 123', email: 'user123@example.com'}

        // Different user - hits API again
        Console.WriteLine("\nRequest 3:");
        string data3 = cachedFetchUserData("456");
        Console.WriteLine(data3);
        // Computing for 456
        //   [API] Fetching data for user 456...
        // (waits 2 seconds)
        // {name: 'User 456', email: 'user456@example.com'}
    }

    public static void TestMemoizationWithLimit()
    {
        // Usage:
        Func<int, int> expensive = n =>
        {
            Thread.Sleep(1000);
            return n * n;
        };

        var memoized = Memoization.MemoizeWithLimit(expensive, maxCacheSize: 3);

        memoized(1); // Computing for 1
        memoized(2); // Computing for 2
        memoized(3); // Computing for 3
        memoized(1); // Cache hit for 1 (updates timestamp)
        memoized(4); // Computing for 4, Cache full - evicted 2 (oldest)
        memoized(2); // Computing for 2 (was evicted)
    }
}