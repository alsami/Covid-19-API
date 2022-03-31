using Covid19Api.Domain;
using Covid19Api.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Covid19Api.ActionFilter;

public class RequestStoreActionFilter : IAsyncActionFilter
{
    private readonly IRequestLogWriteRepository requestLogWriteRepository;

    public RequestStoreActionFilter(IRequestLogWriteRepository requestLogWriteRepository)
    {
        this.requestLogWriteRepository = requestLogWriteRepository;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var requestLog = CreateRequestLog(context);
        await this.requestLogWriteRepository.StoreAsync(requestLog);
        await next();
    }

    private static RequestLog CreateRequestLog(ActionContext context)
    {
        var info = new RequestInfo(context.HttpContext.Request.Scheme,
            context.HttpContext.Request.Protocol, 
            context.HttpContext.Request.Method,
            context.HttpContext.Request.Path, 
            context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "::1");

        return new RequestLog(Guid.NewGuid(), DateTime.UtcNow, info);
    }
}