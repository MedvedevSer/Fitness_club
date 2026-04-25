using Domain.ValueObjects.Base;
using Domain.ValueObjects.Validators;

namespace Domain.ValueObjects;

public class TrainingTime(DateTime startTime, int durationMinutes)
    : ValueObject<(DateTime StartTime, int DurationMinutes)>(new TrainingTimeValidator(), (startTime, durationMinutes))
{
    public DateTime StartTime => Value.StartTime;
    public int DurationMinutes => Value.DurationMinutes;
    public DateTime EndTime => StartTime.AddMinutes(DurationMinutes);
    public bool IsPast => StartTime < DateTime.UtcNow;
    public bool CanBeCancelled => StartTime > DateTime.UtcNow.AddHours(2);

    public bool Overlaps(TrainingTime other)
        => StartTime < other.EndTime && other.StartTime < EndTime;
}