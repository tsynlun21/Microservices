using System.Net;

namespace Infrastructure.Exceptions;

public class BaseException(int code, string message) : Exception(message)
{
    public int Code { get; set; } = code;
}