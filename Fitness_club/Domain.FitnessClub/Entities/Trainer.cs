using Domain.FitnessClub.Base;
using Domain.FitnessClub.Exceptions;
using Domain.ValueObjects;

namespace Domain.FitnessClub.Entities;

public class Trainer(Guid id, Username username) : Entity<Guid>(id)
{
    private readonly ICollection<Training> _trainings = [];

    public Username Username { get; private set; } = username ?? throw new ArgumentNullException(nameof(username));

    public IReadOnlyCollection<Training> Trainings => _trainings.ToList().AsReadOnly();

    protected Trainer() : this(Guid.NewGuid(), null!) { }

    public Trainer(Username username) : this(Guid.NewGuid(), username) { }

    public bool ChangeUsername(Username newUsername)
    {
        if (Username == newUsername) return false;
        Username = newUsername;
        return true;
    }

    public Training CreateTraining(TrainingTitle title, Description description, TrainingTime time, int maxParticipants, string room)
    {
        if (HasTrainingAt(time))
            throw new InvalidOperationException("Trainer already has a training at this time.");

        var training = new Training(this, title, description, time, maxParticipants, room);
        _trainings.Add(training);
        return training;
    }

    public bool EditTraining(Training training, TrainingTitle newTitle, Description newDescription, string newRoom)
    {
        if (training.Trainer != this)
            throw new AnotherTrainerEditTrainingException(training, this);
        if (!_trainings.Contains(training))
            throw new TrainingNotBelongTrainerException(training, this);

        var updated = training.UpdateDetails(newTitle, newDescription, newRoom);
        if (updated) training.SetModificationDate(DateTime.UtcNow);
        return updated;
    }

    public bool RescheduleTraining(Training training, TrainingTime newTime)
    {
        if (training.Trainer != this)
            throw new AnotherTrainerEditTrainingException(training, this);
        if (!_trainings.Contains(training))
            throw new TrainingNotBelongTrainerException(training, this);
        if (HasTrainingAt(newTime, training.Id))
            throw new InvalidOperationException("New time conflicts with another training.");

        var rescheduled = training.Reschedule(newTime);
        if (rescheduled) training.SetModificationDate(DateTime.UtcNow);
        return rescheduled;
    }

    public bool CancelTraining(Training training)
    {
        if (training.Trainer != this)
            throw new AnotherTrainerEditTrainingException(training, this);
        if (!_trainings.Contains(training))
            throw new TrainingNotBelongTrainerException(training, this);

        return training.Cancel();
    }

    private bool HasTrainingAt(TrainingTime time, Guid? excludeId = null)
        => _trainings.Any(t => t.Id != excludeId && t.Time.Overlaps(time));
}