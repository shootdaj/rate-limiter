using System;
using Microsoft.Extensions.Caching.Memory;

namespace RateLimiter.Core.Rules
{
    public class TimespanBetweenRequestsRule : IRule
    {
        public TimespanBetweenRequestsRule(string sourceIdentifier, int ms)
        {
            SourceIdentifier = sourceIdentifier;
            Milliseconds = ms;
        }

        public int Milliseconds { get; set; }

        private static MemoryCache Cache { get; } = new MemoryCache(new MemoryCacheOptions());

        private string CacheIdentifier { get; } = nameof(TimespanBetweenRequestsRule);

        private string SourceIdentifier { get; }

        public bool AllowExecution(string authToken)
        {
            var key = $"{CacheIdentifier}:{SourceIdentifier}:{authToken}";

            if (Cache.TryGetValue(key, out bool value))
            {
                return false;
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMilliseconds(Milliseconds));

            Cache.Set(key, true, cacheEntryOptions);

            return true;
        }

        public string GetNotAllowedReason(string authToken)
        {
            return $"There needs to be {Milliseconds} millisecond(s) between subsequent requests. Please try again later.";
        }
    }
}