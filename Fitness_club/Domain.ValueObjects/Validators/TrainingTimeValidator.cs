using Domain.ValueObjects.Base;
using Domain.ValueObjects.Exceptions;

namespace Domain.ValueObjects.Validators;

public class TrainingTimeValidator : IValidator<(DateTime StartTime, int DurationMinutes)>
{
    public static int MIN_DURATION_MINUTES => 15;
    public static int MAX_DURATION_MINUTES => 180;

    public void Validate((DateTime StartTime, int DurationMinutes) value)
    {
        if (value.StartTime < DateTime.UtcNow)
        {
            throw new ArgumentException("Training time cannot be in the past", nameof(value.StartTime));
        }

        if (value.DurationMinutes < MIN_DURATION_MINUTES)
        {
            throw new ArgumentException($"Duration must be at least {MIN_DURATION_MINUTES} minutes", nameof(value.DurationMinutes));
        }

        if (value.DurationMinutes > MAX_DURATION_MINUTES)
        {
            throw new ArgumentException($"Duration cannot exceed {MAX_DURATION_MINUTES} minutes", nameof(value.DurationMinutes));
        }
    }
}