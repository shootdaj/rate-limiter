using System;

namespace RateLimiter.Core.Rules
{
    public class GeographicMuxRule : IRule
    {
        public GeographicMuxRule(
            string timespanBetweenRequestSourceIdentifier,
            int timespanBetweenRequestsMs,
            int requestsPerTimespanMaxRequestCount,
            int requestsPerTimespanSeconds)
        {
            TimespanPerRequestsSourceIdentifier = timespanBetweenRequestSourceIdentifier;
            RequestsPerTimespanMaxRequestCount = requestsPerTimespanMaxRequestCount;
            RequestsPerTimespanSeconds = requestsPerTimespanSeconds;
            TimespanBetweenRequestsMS = timespanBetweenRequestsMs;
        }

        private string TimespanPerRequestsSourceIdentifier { get; }

        private int RequestsPerTimespanMaxRequestCount { get; }

        private int RequestsPerTimespanSeconds { get; }

        private int TimespanBetweenRequestsMS { get; }

        private readonly string TokenErrorMessage = $"Auth token must begin with '{US}' or '{EU}'.";

        public static readonly string US = "US";

        public static readonly string EU = "EU";

        public bool AllowExecution(string authToken)
        {
            if (authToken.StartsWith(US))
            {
                return new RequestsPerTimespanRule(RequestsPerTimespanMaxRequestCount, RequestsPerTimespanSeconds).AllowExecution(authToken);
            }
            else if (authToken.StartsWith(EU))
            {
                return new TimespanBetweenRequestsRule(TimespanPerRequestsSourceIdentifier, TimespanBetweenRequestsMS).AllowExecution(authToken);
            }

            throw new ArgumentException(TokenErrorMessage);
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
                return new TimespanBetweenRequestsRule(TimespanPerRequestsSourceIdentifier, TimespanBetweenRequestsMS).GetNotAllowedReason(authToken);
            }

            throw new ArgumentException(TokenErrorMessage);
        }
    }
}