using System.Text;
using System.Text.Json;
using Infrastructure.Exceptions;
using Infrastructure.Models;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Infrastructure.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception e)
        {
            await HandleException(httpContext, e);
        }
    }

    private async Task HandleException(HttpContext httpContext, Exception exception)
    {
        await LogError(httpContext.Request, exception);

        var exceptionFault = exception as RequestFaultException;

        int code = StatusCodes.Status500InternalServerError;
        string message = exception.Message;
        if (exceptionFault != null)
        {
            var requestExceptionTypeName = exceptionFault.Fault?.Exceptions.FirstOrDefault()?.ExceptionType ?? string.Empty;
            
            if (requestExceptionTypeName == typeof(NotFoundException).FullName)
                code = StatusCodes.Status404NotFound;
            if (requestExceptionTypeName == typeof(BadRequestException).FullName)
                code = StatusCodes.Status400BadRequest;
            message = exceptionFault.Fault?.Exceptions.FirstOrDefault()?.Message ?? message;
        }

        var result = JsonSerializer.Serialize(ApiResult<string>.Failure(code, message),
            new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseUpper,
            });

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode  = code;

        await httpContext.Response.WriteAsync(result);
    }

    private async Task LogError(HttpRequest httpContextRequest, Exception exception)
    {
        var requestPath = httpContextRequest.Path;

        var requestMethod = httpContextRequest.Method;

        var queryParameters = httpContextRequest.QueryString.HasValue
            ? httpContextRequest.QueryString.Value
            : String.Empty;

        var headers = httpContextRequest.Headers
            .ToDictionary(h => h.Key, h => h.Value.ToString());

        string requestBody = null;
        if (httpContextRequest.Body.CanRead)
        {
            requestBody = await new StreamReader(httpContextRequest.Body).ReadToEndAsync();
            //httpContextRequest.Body.Position = 0;

            //requestBody = JsonSerializer.Serialize(httpContextRequest.Body);

            //httpContextRequest.Body.Position = 0;
        }

        var logMessage = $"""
                          Request Path: {requestPath}
                          Method: {requestMethod}
                          Query Parameters: {queryParameters}
                          Headers: {string.Join(", ", headers.Select(h => $"{h.Key}: {h.Value}"))}
                          Exception: {exception.Message}
                          Request body: {requestBody ?? String.Empty}
                          StackTrace: {exception.StackTrace}
                          """;

        logger.LogError(exception, logMessage);
    }
}