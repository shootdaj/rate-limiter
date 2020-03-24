using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace RateLimiter.API
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TimespanBetweenRequests : ActionFilterAttribute
    {
        public TimeSpan TimeSpan { get; set; }

        private static MemoryCache Cache { get; set; } = new MemoryCache(new MemoryCacheOptions());

        public override void OnActionExecuting(ActionExecutingContext context)
        {


            base.OnActionExecuting(context);
        }
    }
}
