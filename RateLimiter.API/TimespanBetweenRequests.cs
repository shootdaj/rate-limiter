using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RateLimiter.API
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TimespanBetweenRequestsAttribute : ActionFilterAttribute
    {
        private TimespanBetweenRequestsRule Rule { get; }

        public TimespanBetweenRequestsAttribute(int ms)
        {
            Rule = new TimespanBetweenRequestsRule(ms);
        }
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("AuthToken", out object authToken))
            {
                if (!Rule.AllowExecution((string)authToken))
                {
                    context.Result = new ContentResult
                    {
                        Content = "Too many requests. Try again later."
                    };

                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.TooManyRequests;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
