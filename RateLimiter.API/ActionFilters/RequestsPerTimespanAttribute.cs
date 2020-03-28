using System;
using RateLimiter.Core.Rules;

namespace RateLimiter.API.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequestsPerTimespanAttribute : RateLimiterAttributeBase
    {
        public RequestsPerTimespanAttribute(int maxRequestCount, int seconds)
        {
            Rule = new RequestsPerTimespanRule(maxRequestCount, seconds);
        }
    }
}
