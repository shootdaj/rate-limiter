using System;
using Microsoft.Extensions.Caching.Memory;

namespace RateLimiter
{
    public class TimespanBetweenRequestsRule : IRule
    {
        public TimespanBetweenRequestsRule(int ms)
        {
            Milliseconds = ms;
        }

        public int Milliseconds { get; set; }

        private static MemoryCache Cache { get; } = new MemoryCache(new MemoryCacheOptions());

        private static string CacheIdentifier { get; } = nameof(TimespanBetweenRequestsRule);

        public bool AllowExecution(string authToken)
        {
            var key = $"{CacheIdentifier}:{authToken}";

            if (Cache.TryGetValue(key, out bool value))
            {
                return false;
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMilliseconds(Milliseconds));

            Cache.Set(key, true, cacheEntryOptions);

            return true;
        }
    }
}