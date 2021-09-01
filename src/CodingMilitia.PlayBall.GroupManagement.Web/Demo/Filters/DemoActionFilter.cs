using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Demo.Filters
{
    public class DemoActionFilter : IActionFilter
    {
        private readonly ILogger<DemoActionFilter> _logger;

        public DemoActionFilter(ILogger<DemoActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Before action execution. Action: {actionName}. ModelState: {@modelState}. ActionArguments: {@actionArguments}", context.ActionDescriptor.DisplayName, context.ModelState, context.ActionArguments);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("After action execution. Action: {actionName}.", context.ActionDescriptor.DisplayName, context.Result);
        }

    }
}
