using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using RateLimiter.Core.Rules;
using Xunit;

namespace RateLimiter.Core.Tests
{
    public class TimespanBetweenRequestsRuleTests
    {
        [ClassData(typeof(AllowExecutionTestData))]
        [Theory]
        public async Task AllowExecution_Works(int ms, TimespanBetweenRequestTimeline[] timeline)
        {
            var sourceIdentifier = Guid.NewGuid().ToString();
            var timespanBetweenRequestsRule = new TimespanBetweenRequestsRule(sourceIdentifier, ms);
            
            foreach (var request in timeline)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(request.DelayMS));
                var actual = timespanBetweenRequestsRule.AllowExecution(request.AuthToken);

                Assert.Equal(request.Expected, actual);
            }
        }
    }

    class AllowExecutionTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                1000,
                new[]
                {
                    new TimespanBetweenRequestTimeline(0, true, "AuthToken"),
                    new TimespanBetweenRequestTimeline(100, false, "AuthToken"),
                    new TimespanBetweenRequestTimeline(400, false, "AuthToken"),
                    new TimespanBetweenRequestTimeline(300, false, "AuthToken"),
                    new TimespanBetweenRequestTimeline(600, true, "AuthToken"),
                }
            };

            yield return new object[]
            {
                500,
                new[]
                {
                    new TimespanBetweenRequestTimeline(0, true, "AuthToken"),
                    new TimespanBetweenRequestTimeline(100, false, "AuthToken"),
                    new TimespanBetweenRequestTimeline(200, false, "AuthToken"),
                    new TimespanBetweenRequestTimeline(100, false, "AuthToken"),
                    new TimespanBetweenRequestTimeline(200, true, "AuthToken"),
                }
            };

            yield return new object[]
            {
                100,
                new[]
                {
                    new TimespanBetweenRequestTimeline(0, true, "AuthToken"),
                    new TimespanBetweenRequestTimeline(50, false, "AuthToken"),
                    new TimespanBetweenRequestTimeline(10, false, "AuthToken"),
                    new TimespanBetweenRequestTimeline(10, false, "AuthToken"),
                    new TimespanBetweenRequestTimeline(40, true, "AuthToken"),
                    new TimespanBetweenRequestTimeline(50, false, "AuthToken"),
                    new TimespanBetweenRequestTimeline(60, true, "AuthToken"),
                }
            };

            yield return new object[]
            {
                1000,
                new[]
                {
                    new TimespanBetweenRequestTimeline(0, true, "AuthToken"),
                    new TimespanBetweenRequestTimeline(500, false, "AuthToken"), //50
                    new TimespanBetweenRequestTimeline(100, true, "AuthToken2"), //60
                    new TimespanBetweenRequestTimeline(100, false, "AuthToken"), //70
                    new TimespanBetweenRequestTimeline(100, false, "AuthToken2"), //80
                    new TimespanBetweenRequestTimeline(100, false, "AuthToken"), //90
                    new TimespanBetweenRequestTimeline(200, true, "AuthToken"), //110
                    new TimespanBetweenRequestTimeline(100, false, "AuthToken2"), //120
                    new TimespanBetweenRequestTimeline(500, false, "AuthToken"), //170
                    new TimespanBetweenRequestTimeline(100, true, "AuthToken2"), //180
                    new TimespanBetweenRequestTimeline(600, true, "AuthToken"), //240
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class TimespanBetweenRequestTimeline
    {
        public TimespanBetweenRequestTimeline(int delayMS, bool expected, string authToken)
        {
            DelayMS = delayMS;
            Expected = expected;
            AuthToken = authToken;
        }

        public string AuthToken { get; }
        public int DelayMS { get; }
        public bool Expected { get; }
    }
}
