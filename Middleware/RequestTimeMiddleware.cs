public class RequestTimeMiddleware
{
    private readonly RequestDelegate _next;

    public RequestTimeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync (HttpContext context)
    {
        DateTime startTime = DateTime.Now;

        await _next(context);

        DateTime endTime = DateTime.Now;
        double requestTotalTime = (endTime - startTime).TotalMilliseconds;

        Console.WriteLine($"{context.Request.Method}  {context.Request.Path} - Status: {context.Response.StatusCode}/Request Time: {requestTotalTime}ms");
    }
}