using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Models;

public class ApiResult<T>(bool isSuccess, int statusCode, T? result, string? exception = null)
{
    public string? Exception { get; set; } = exception;

    public T? Result { get; set; } = result;

    public int StatusCode { get; set; } = statusCode;

    public bool IsSuccess { get; set; } = isSuccess;
    
    public static ApiResult<T> Success200(T result) => new (true, StatusCodes.Status200OK, result);
    
    public static ApiResult<T> Failure(int code, string exception) => new (false, code, default, exception);
}