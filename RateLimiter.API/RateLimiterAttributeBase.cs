using System.Data;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RateLimiter.API
{
    public class RateLimiterAttributeBase : ActionFilterAttribute
    {
        protected IRule Rule { get; set; }

        protected string ExceptionMessage { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.TryGetValue("AuthToken", out object authToken))
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            if (!Rule.AllowExecution((string) authToken))
            {
                context.Result = new ContentResult
                {
                    Content = ExceptionMessage
                };

                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.TooManyRequests;
            }
        }
    }
}