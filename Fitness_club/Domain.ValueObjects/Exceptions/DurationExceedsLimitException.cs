namespace Domain.ValueObjects.Exceptions;

internal class DurationExceedsLimitException(int duration, int maxDuration)
    : ArgumentException($"Duration {duration} exceeds maximum allowed {maxDuration}")
{
    public int Duration => duration;
    public int MaxDuration => maxDuration;
}