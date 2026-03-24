using Domain.ValueObjects.Base;
using Domain.ValueObjects.Exceptions;

namespace Domain.ValueObjects.Validators;

public class TrainingTimeValidator : IValidator<(DateTime StartTime, int DurationMinutes)>
{
    public void Validate((DateTime StartTime, int DurationMinutes) value)
    {
        if (value.StartTime < DateTime.UtcNow)
            throw new ArgumentException("Training time cannot be in the past", nameof(value.StartTime));

        if (value.DurationMinutes < 15)
            throw new ArgumentException("Duration must be at least 15 minutes", nameof(value.DurationMinutes));

        if (value.DurationMinutes > 180)
            throw new ArgumentException("Duration cannot exceed 180 minutes", nameof(value.DurationMinutes));
    }
}