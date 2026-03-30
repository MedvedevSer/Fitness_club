using Domain.FitnessClub.Base;
using Domain.FitnessClub.Exceptions;
using Domain.ValueObjects;

namespace Domain.FitnessClub.Entities;

public class Trainer : Entity<Guid>
{
    public Username Username { get; private set; }

    private readonly List<Training> _trainings = new();
    public IReadOnlyCollection<Training> Trainings => _trainings.AsReadOnly();

    protected Trainer()
    {
        Username = null!;
    }

    public Trainer(Username username) : this(Guid.NewGuid(), username)
    {
    }

    public Trainer(Guid id, Username username) : base(id)
    {
        Username = username ?? throw new ArgumentNullException(nameof(username));
    }

    public bool ChangeUsername(Username newUsername)
    {
        if (newUsername == null) throw new ArgumentNullException(nameof(newUsername));
        if (Username == newUsername) return false;

        Username = newUsername;
        return true;
    }

    public Training CreateTraining(
        TrainingTitle title,
        Description description,
        TrainingTime time,
        int maxParticipants,
        string room)
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

        var isEdited = training.UpdateDetails(newTitle, newDescription, newRoom);
        if (isEdited) training.SetModificationDate(DateTime.UtcNow);
        return isEdited;
    }

    public bool RescheduleTraining(Training training, TrainingTime newTime)
    {
        if (training.Trainer != this)
            throw new AnotherTrainerEditTrainingException(training, this);
        if (!_trainings.Contains(training))
            throw new TrainingNotBelongTrainerException(training, this);
        if (HasTrainingAt(newTime, training.Id))
            throw new InvalidOperationException("New time conflicts with another training.");

        var isRescheduled = training.Reschedule(newTime);
        if (isRescheduled) training.SetModificationDate(DateTime.UtcNow);
        return isRescheduled;
    }

    public bool CancelTraining(Training training)
    {
        if (training.Trainer != this)
            throw new AnotherTrainerEditTrainingException(training, this);
        if (!_trainings.Contains(training))
            throw new TrainingNotBelongTrainerException(training, this);

        return training.Cancel();
    }

    private bool HasTrainingAt(TrainingTime time, Guid? excludeTrainingId = null)
    {
        return _trainings.Any(t => t.Id != excludeTrainingId && t.Time.Overlaps(time));
    }
}