namespace Domain.ValueObjects.Exceptions;

internal class ArgumentNullOrWhiteSpaceException(string paramName, string message)
    : ArgumentNullException(paramName, message);