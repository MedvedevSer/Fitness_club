namespace Domain.ValueObjects.Exceptions;

internal class DurationNonPositiveException(int duration)
    : ArgumentException($"Duration {duration} must be positive")
{
    public int Duration => duration;
}