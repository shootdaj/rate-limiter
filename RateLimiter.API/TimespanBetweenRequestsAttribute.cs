using System;
using RateLimiter.Core;

namespace RateLimiter.API
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TimespanBetweenRequestsAttribute : RateLimiterAttributeBase
    {
        public TimespanBetweenRequestsAttribute(int ms)
        {
            Rule = new TimespanBetweenRequestsRule(ms);
            ExceptionMessage = $"There needs to be {ms} seconds between subsequent requests.";
        }
    }
}
