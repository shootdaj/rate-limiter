using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateLimiter
{

    [AttributeUsage(AttributeTargets.Method)]
    public class TimespanBetweenRequests : ActionFilterAttribute
    {

    }
}
