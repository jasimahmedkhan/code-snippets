using Microsoft.Extensions.Caching.Memory;

namespace design_patterns.SingletonPattern;

public sealed class CacheManager
{
    private static readonly Lazy<CacheManager> _instance =
        new Lazy<CacheManager>(() => new CacheManager());

    private readonly MemoryCache _cache;

    private CacheManager()
    {
        _cache = new MemoryCache(new MemoryCacheOptions());
    }

    public static CacheManager Instance => _instance.Value;

    public T Get<T>(string key)
    {
        return _cache.Get<T>(key);
    }

    public void Set<T>(string key, T value, TimeSpan expiration)
    {
        _cache.Set(key, value, expiration);
    }
}

public class CacheManagerTest
{
    public static void CacheManagerTestMain()
    {
        var cacheManager = CacheManager.Instance;
        cacheManager.Set("key", "value", TimeSpan.FromSeconds(3));
        cacheManager.Set("key2", "value2", TimeSpan.FromSeconds(10));
        Thread.Sleep(3000);
        Console.WriteLine(cacheManager.Get<string>("key2"));
        Console.WriteLine(cacheManager.Get<string>("key"));
        var value = cacheManager.Get<string>("key");
        Console.WriteLine(value);
    }
}