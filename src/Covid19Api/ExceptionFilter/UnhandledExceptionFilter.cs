using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

// ReSharper disable ClassNeverInstantiated.Global

namespace Covid19Api.ExceptionFilter
{
    public class UnhandledExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<UnhandledExceptionFilter> logger;

        public UnhandledExceptionFilter(ILogger<UnhandledExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled) return;

            this.logger.LogCritical(context.Exception, context.Exception.Message);
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(new
            {
                context.Exception.Message
            });
        }
    }
}