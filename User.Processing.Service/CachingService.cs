using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using User.Processing.Service.Interface;

namespace User.Processing.Service
{
    public class CachingService : ICachingService
    {
        private readonly IConfiguration _configuration;
        private readonly ConcurrentDictionary<string, (object Value, DateTime Expiry)> _cache = new();

        public CachingService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SetAsync(string key, object value, TimeSpan? absoluteExpiration = null)
        {
            int defaultMinutes = _configuration.GetValue<int>("Cache:DefaultMinutes", 1);
            var expiry = DateTime.UtcNow.Add(absoluteExpiration ?? TimeSpan.FromMinutes(defaultMinutes));
            _cache[key] = (value, expiry);
            return Task.CompletedTask;
        }

        public Task<T?> GetAsync<T>(string key)
        {
            if (_cache.TryGetValue(key, out var entry))
            {
                if (entry.Expiry > DateTime.UtcNow && entry.Value is T tValue)
                {
                    return Task.FromResult<T?>(tValue);
                }
                _cache.TryRemove(key, out _);
            }
            return Task.FromResult<T?>(default);
        }
    }
}