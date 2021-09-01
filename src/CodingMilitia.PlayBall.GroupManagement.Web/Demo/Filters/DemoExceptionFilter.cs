using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Demo.Filters
{
    public class DemoExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<DemoActionFilter> _logger;

        public DemoExceptionFilter(ILogger<DemoActionFilter> logger)
        {
            _logger = logger;
        }

        public string Suffix { get; internal set; } = "by default";

        public void OnException(ExceptionContext context)
        {
            if(context.Exception is ArgumentException)
            {
                _logger.LogError("Transforming {exceptionTransformed} into {result}. Suffix {suffix}", nameof(ArgumentException), nameof(BadRequestResult), Suffix);

                // affecting the result will short circuit filter pipeline
                context.Result = new BadRequestResult();
            }
        }
    }
}
