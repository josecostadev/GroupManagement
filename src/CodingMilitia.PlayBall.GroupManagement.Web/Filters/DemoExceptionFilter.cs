using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Demo.Filters
{
    public class ConflictExceptionActionFilter : IExceptionFilter
    {
        private readonly ILogger<ConflictExceptionActionFilter> _logger;

        public ConflictExceptionActionFilter(ILogger<ConflictExceptionActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if(context.Exception is DbUpdateConcurrencyException)
            {
                _logger.LogError("Exception filter triggered for {exceptionType}", nameof(DbUpdateConcurrencyException));
                context.Result = new ConflictObjectResult(new { Message = "Database conflict exception" });
            }
            else
            {
                _logger.LogError("Exception filter triggered: {exception}", context.Exception);
            }
        }
    }
}
