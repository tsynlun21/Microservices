using Microsoft.AspNetCore.Http;

namespace Infrastructure.Models;

public class ApiResult<T>
{
    public ApiResult(bool isSuccess, int statusCode, T result, List<string> errors)
    {
        IsSuccess  = isSuccess;
        StatusCode = statusCode;
        Result     = result;
        Errors     = errors;
    }

    public List<string> Errors { get; set; }

    public T Result { get; set; }

    public int StatusCode { get; set; }

    public bool IsSuccess { get; set; }
    
    public static ApiResult<T> Success(int code, T result) => new ApiResult<T>(true, code, result, Array.Empty<string>().ToList());
    
    public static ApiResult<T> Success200(T result) => new (true, StatusCodes.Status200OK, result, Array.Empty<string>().ToList());
    
    public static ApiResult<T> Failure(int code, List<string> errors) => new ApiResult<T>(false, code, default, errors);
}