namespace Domain.ValueObjects.Exceptions;

internal class TitleLongValueException(string title, int maxLength)
    : FormatException($"Title length {title.Length} greater than maximum allowed length {maxLength}")
{
    public string Title => title;
    public int MaxLength => maxLength;
}