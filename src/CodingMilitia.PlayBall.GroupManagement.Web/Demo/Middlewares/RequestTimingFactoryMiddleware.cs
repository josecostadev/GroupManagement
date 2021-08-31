using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Demo.Middlewares
{
    public class RequestTimingFactoryMiddleware : IMiddleware
    {
        private readonly ILogger<RequestTimingFactoryMiddleware> _logger;
        private int _requestsCouter = 0;

        public RequestTimingFactoryMiddleware(ILogger<RequestTimingFactoryMiddleware> logger)
        {
            _logger = logger;
        }

        // Remember: dependencies should come here instead of constructor injection. This is NEEDED
        // This will be a singleton
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var sw = new Stopwatch();

            sw.Start();

            await next(context);

            sw.Stop();

            ++_requestsCouter;

            _logger.LogTrace("### Request took {requestElapsedTime} ms. Requests processed by this intance: {numberOfRequests}", sw.ElapsedMilliseconds, _requestsCouter);
        }
    }
}
