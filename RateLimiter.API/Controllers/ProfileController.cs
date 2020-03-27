using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RateLimiter.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(ILogger<ProfileController> logger)
        {
            _logger = logger;
        }

        [TimespanBetweenRequests(5000)]
        [HttpGet]
        [Route("name")]
        public async Task<string> GetName(string authToken)
        {
            return "Anshul";
        }

        [TimespanBetweenRequests(5000)]
        [HttpGet]
        [Route("age")]
        public async Task<int> GetAge(string authToken)
        {
            return 32;
        }
    }
}
