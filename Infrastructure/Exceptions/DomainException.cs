using System.Net;

namespace Infrastructure.Exceptions;

public class DomainException(int code, string message) : BaseException(code, message)
{
    
}