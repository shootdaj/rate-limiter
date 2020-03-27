using System;
using RateLimiter.Core;

namespace RateLimiter.API
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequestsPerTimespanAttribute : RateLimiterAttributeBase
    {
        public RequestsPerTimespanAttribute(int maxRequestCount, int ms)
        {
            Rule = new RequestsPerTimespanRule(maxRequestCount, ms);
            ExceptionMessage = $"Up to {maxRequestCount} requests per {ms} milliseconds are allowed.";
        }
    }
}
