using System;
using RateLimiter.Core;
using RateLimiter.Core.Rules;

namespace RateLimiter.API.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TimespanBetweenRequestsAttribute : RateLimiterAttributeBase
    {
        public TimespanBetweenRequestsAttribute(int ms)
        {
            Rule = new TimespanBetweenRequestsRule(ms);
        }
    }
}
