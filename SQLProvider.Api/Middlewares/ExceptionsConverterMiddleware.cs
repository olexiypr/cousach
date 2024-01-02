using System.Net;
using System.Text.Json;
using SQLProvider.Data.Exceptions;

namespace SQLProvider.Api.Middlewares;

public class ExceptionsConverterMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionsConverterMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            await ConvertExceptionAsync(context, ex);
        }
    }

    private Task ConvertExceptionAsync(HttpContext context, Exception ex)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;
        switch (ex)
        {
            case InvalidConnectionStringException invalidConnectionStringException:
                code = HttpStatusCode.BadRequest;
                break;
            case ConnectionNotFoundException connectionNotFoundException:
                code = HttpStatusCode.NotFound;
                break;
        }
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        if (result == string.Empty)
        {
            result = JsonSerializer.Serialize(new {error = ex.Message});
        }

        return context.Response.WriteAsync(result);
    }
}