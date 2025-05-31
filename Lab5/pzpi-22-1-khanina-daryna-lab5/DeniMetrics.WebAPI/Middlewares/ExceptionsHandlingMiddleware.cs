using System.Net;
using DineMetrics.Core.Dto;

namespace DeniMetrics.WebAPI.Middlewares;

public class ExceptionsHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionsHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext httpContext,
        HttpStatusCode httpStatusCode,
        string message)
    {
        var response = httpContext.Response;

        response.ContentType = "application/json";
        response.StatusCode = (int)httpStatusCode;

        ErrorDto errorDto = new()
        {
            Message = message,
            StatusCode = (int)httpStatusCode
        };

        var result = errorDto.ToString();

        await response.WriteAsync(result);
    }
}