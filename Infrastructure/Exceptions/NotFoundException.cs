using System.Net;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Exceptions;

public class NotFoundException(string message) : DomainException(StatusCodes.Status404NotFound ,message);