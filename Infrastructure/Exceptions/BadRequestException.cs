namespace Infrastructure.Exceptions;

public class BadRequestException(string message) : Exception(message);