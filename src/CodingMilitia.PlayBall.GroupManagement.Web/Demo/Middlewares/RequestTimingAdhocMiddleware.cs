using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Demo.Middlewares
{
    public class RequestTimingAdhocMiddleware
    {
        private readonly RequestDelegate _next;

        private int _requestsCouter = 0;

        public RequestTimingAdhocMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Remember: dependencies should come here instead of constructor injection. This is NEEDED
        // This will be a singleton
        public async Task InvokeAsync(HttpContext context, ILogger<RequestTimingAdhocMiddleware> logger)
        {            
            var sw = new Stopwatch();
            
            sw.Start();

            await _next(context);

            sw.Stop();

            ++_requestsCouter;

            logger.LogTrace("### Request took {requestElapsedTime} ms. Requests processed by this intance: {numberOfRequests}", sw.ElapsedMilliseconds, _requestsCouter);
        }
    }
}
