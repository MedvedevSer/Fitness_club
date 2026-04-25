namespace Domain.ValueObjects.Exceptions;

internal class DescriptionLongValueException(string description, int maxLength)
    : FormatException($"Description length {description.Length} greater than maximum allowed length {maxLength}")
{
    public string Description => description;
    public int MaxLength => maxLength;
}