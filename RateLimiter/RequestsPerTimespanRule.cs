using System;
using System.Collections.Concurrent;
using System.Runtime.Caching;

namespace RateLimiter
{
    public class RequestsPerTimespanRule : IRule
    {
        public RequestsPerTimespanRule(int maxRequestCount, int seconds)
        {
            MaxRequestCount = maxRequestCount;
            Seconds = seconds;
        }

        public int MaxRequestCount { get; set; }

        public int Seconds { get; set; }

        private static ConcurrentDictionary<string, int> RequestBuckets { get; } = new ConcurrentDictionary<string, int>();
        
        private static string CacheIdentifier { get; } = nameof(RequestsPerTimespanRule);

        private static ObjectCache RequestTimers { get; } = new MemoryCache(CacheIdentifier);

        public bool AllowExecution(string authToken)
        {
            var key = $"{CacheIdentifier}:{authToken}";

            if (RequestBuckets.TryGetValue(key, out var activeRequests))
            {
                if (activeRequests < MaxRequestCount)
                {
                    IncrementRequestBucket(key, activeRequests);
                    return true;
                }

                return false;
            }

            IncrementRequestBucket(key, 0);

            return true;
        }

        private void IncrementRequestBucket(string key, int activeRequests)
        {
            RequestBuckets[key] = activeRequests + 1;
            RequestTimers.Set(Guid.NewGuid().ToString(), key, new CacheItemPolicy()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(Seconds),
                RemovedCallback = arguments =>
                {
                    if (RequestBuckets[key] <= 1)
                    {
                        RequestBuckets.TryRemove(key, out var value);
                    }
                    else
                    {
                        RequestBuckets[key]--;
                    }
                }
            });
        }
    }
}