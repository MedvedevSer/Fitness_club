using Domain.ValueObjects.Base;
using Domain.ValueObjects.Exceptions;

namespace Domain.ValueObjects.Validators;

public class TrainingTimeValidator : IValidator<(DateTime StartTime, int DurationMinutes)>
{
    public const int MAX_DURATION_MINUTES = 180;

    public void Validate((DateTime StartTime, int DurationMinutes) value)
    {
        if (value.StartTime == default)
            throw new ArgumentException(ExceptionMessages.START_TIME_REQUIRED, nameof(value.StartTime));
        if (value.DurationMinutes <= 0)
            throw new DurationNonPositiveException(value.DurationMinutes);
        if (value.DurationMinutes > MAX_DURATION_MINUTES)
            throw new DurationExceedsLimitException(value.DurationMinutes, MAX_DURATION_MINUTES);
    }
}