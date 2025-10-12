
// File: Services/CacheService.cs
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System;

namespace OutlookBlazorTestApp2.services
{
    public class CacheService
    {
        private readonly ConcurrentDictionary<string, object> _cache = new();

        public Task<T?> GetAsync<T>(string key)
        {
            if (_cache.TryGetValue(key, out var value) && value is T tValue)
            {
                return Task.FromResult<T?>(tValue);
            }
            return Task.FromResult<T?>(default);
        }

        public Task SetAsync<T>(string key, T value)
        {
            _cache[key] = value!;
            return Task.CompletedTask;
        }
    }
}
