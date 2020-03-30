using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Azure.Cosmos;
// ReSharper disable ClassNeverInstantiated.Global

namespace Covid19Api.ExceptionFilter
{
    public class AzureCosmosDbThrottleExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (!(context.Exception.InnerException is CosmosException)) return;

            context.ExceptionHandled = true;

            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.Result = new ObjectResult(new
            {
                Message = "Too many request to Cosmos DB"
            });
        }
    }
}