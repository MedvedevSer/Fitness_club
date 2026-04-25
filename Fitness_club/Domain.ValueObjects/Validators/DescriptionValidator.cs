using Domain.ValueObjects.Base;
using Domain.ValueObjects.Exceptions;

namespace Domain.ValueObjects.Validators;

public class DescriptionValidator : IValidator<string>
{
    public const int MAX_LENGTH = 500;

    public void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullOrWhiteSpaceException(nameof(value), ExceptionMessages.DESCRIPTION_NOT_NULL_OR_WHITE_SPACE);
        if (value.Length > MAX_LENGTH)
            throw new DescriptionLongValueException(value, MAX_LENGTH);
    }
}