using Domain.ValueObjects.Base;
using Domain.ValueObjects.Exceptions;

namespace Domain.ValueObjects.Validators;

public class UsernameValidator : IValidator<string>
{
    public const int MAX_LENGTH = 20;

    public void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullOrWhiteSpaceException(nameof(value), ExceptionMessages.USERNAME_NOT_NULL_OR_WHITE_SPACE);
        if (value.Length > MAX_LENGTH)
            throw new UsernameLongValueException(value, MAX_LENGTH);
    }
}