using System;

namespace RateLimiter.Core.Rules
{
    public class GeographicMuxRule : IRule
    {
        public GeographicMuxRule(
            int timespanBetweenRequestsMs,
            int requestsPerTimespanMaxRequestCount,
            int requestsPerTimespanSeconds)
        {
            RequestsPerTimespanMaxRequestCount = requestsPerTimespanMaxRequestCount;
            RequestsPerTimespanSeconds = requestsPerTimespanSeconds;
            TimespanBetweenRequestsMS = timespanBetweenRequestsMs;
        }

        private int RequestsPerTimespanMaxRequestCount { get; }

        private int RequestsPerTimespanSeconds { get; }

        private int TimespanBetweenRequestsMS { get; }

        public static string US = "US";

        public static string EU = "EU";

        public bool AllowExecution(string authToken)
        {
            if (authToken.StartsWith(US))
            {
                return new RequestsPerTimespanRule(RequestsPerTimespanMaxRequestCount, RequestsPerTimespanSeconds).AllowExecution(authToken);
            }
            else if (authToken.StartsWith(EU))
            {
                return new TimespanBetweenRequestsRule(TimespanBetweenRequestsMS).AllowExecution(authToken);
            }

            throw new ArgumentException($"Auth token must begin with '{US}' or '{EU}'.");
        }

        public string GetNotAllowedReason(string authToken)
        {
            if (authToken.StartsWith(US))
            {
                return new RequestsPerTimespanRule(RequestsPerTimespanMaxRequestCount, RequestsPerTimespanSeconds)
                    .GetNotAllowedReason(authToken);
            }
            else if (authToken.StartsWith(EU))
            {
                return new TimespanBetweenRequestsRule(TimespanBetweenRequestsMS).GetNotAllowedReason(authToken);
            }

            throw new ArgumentException($"Auth token must begin with '{US}' or '{EU}'");
        }
    }
}