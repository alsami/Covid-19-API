using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Covid19Api.ExceptionFilter
{
    public class UnhandledExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new OkObjectResult(new
            {
                Error = context.Exception.Message
            });

            context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
        }
    }
}