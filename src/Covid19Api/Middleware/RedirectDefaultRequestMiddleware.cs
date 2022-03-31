namespace Covid19Api.Middleware;

internal class RedirectDefaultRequestMiddleware : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path == new PathString("/"))
        {
            context.Request.Path = new PathString("/swagger");
        }

        return next(context);
    }
}