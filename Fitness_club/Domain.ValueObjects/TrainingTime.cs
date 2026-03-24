using Domain.ValueObjects.Base;
using Domain.ValueObjects.Validators;

namespace Domain.ValueObjects;

public class TrainingTime : ValueObject<(DateTime StartTime, int DurationMinutes)>
{
    public DateTime StartTime => Value.StartTime;
    public int DurationMinutes => Value.DurationMinutes;
    public DateTime EndTime => StartTime.AddMinutes(DurationMinutes);
    public bool IsPast => StartTime < DateTime.UtcNow;
    public bool CanBeCancelled => StartTime > DateTime.UtcNow.AddHours(2);

    public TrainingTime(DateTime startTime, int durationMinutes)
        : base(new TrainingTimeValidator(), (startTime, durationMinutes))
    {
    }

    public bool Overlaps(TrainingTime other)
    {
        return StartTime < other.EndTime && other.StartTime < EndTime;
    }
}