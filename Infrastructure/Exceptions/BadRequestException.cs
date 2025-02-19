using System.Net;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Exceptions;

public class BadRequestException(string message) : DomainException(StatusCodes.Status400BadRequest, message);