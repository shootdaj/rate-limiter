using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateLimiter
{
    public class ResourceInvoker
    {
        public async Task InvokeResource(Resource resource, string authToken, params object[] arguments)
        {
            switch (resource)
            {
                case Resource.GetNames:
                    if 
                default:
                    throw new Exception("Unsupported resource");
            }
        }
    }

    public enum Resource
    {
        GetNames = 0
    }
}
