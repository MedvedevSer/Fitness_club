using Domain.ValueObjects.Base;
using Domain.ValueObjects.Exceptions;

namespace Domain.ValueObjects.Validators;

public class DescriptionValidator : IValidator<string>
{
    public static int MAX_LENGTH => 500;

    public void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullOrWhiteSpaceException(nameof(value));
        }

        if (value.Length > MAX_LENGTH)
        {
            throw new ArgumentLongValueException(
                paramName: nameof(value),
                value: value,
                maxLength: MAX_LENGTH);
        }
    }
}