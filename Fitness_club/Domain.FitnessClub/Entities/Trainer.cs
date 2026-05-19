using Domain.FitnessClub.Base;
using Domain.FitnessClub.Exceptions;
using Domain.ValueObjects;

namespace Domain.FitnessClub.Entities;

public class Trainer : Entity<Guid>
{
    private readonly ICollection<Training> _trainings = [];

    public Username Username { get; private set; }

    public IReadOnlyCollection<Training> Trainings => _trainings.ToList().AsReadOnly();

    protected Trainer() { }

    public Trainer(Guid id, Username username) : base(id)
    {
        Username = username ?? throw new ArgumentNullValueException(nameof(username));
    }

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
            throw new TrainerAlreadyHasTrainingAtTimeException(this, time);

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

        return training.UpdateDetails(this, newTitle, newDescription, newRoom);
    }

    public bool RescheduleTraining(Training training, TrainingTime newTime)
    {
        if (training.Trainer != this)
            throw new AnotherTrainerEditTrainingException(training, this);
        if (!_trainings.Contains(training))
            throw new TrainingNotBelongTrainerException(training, this);
        if (HasTrainingAt(newTime, training))
            throw new TrainerAlreadyHasTrainingAtTimeException(this, newTime);

        return training.Reschedule(this, newTime);
    }

    public bool CancelTraining(Training training)
    {
        if (training.Trainer != this)
            throw new AnotherTrainerEditTrainingException(training, this);
        if (!_trainings.Contains(training))
            throw new TrainingNotBelongTrainerException(training, this);
        if (training.Time.IsPast)
            throw new CannotCancelPastTrainingException(training);

        return training.Cancel(this);
    }

    public bool CompleteTraining(Training training)
    {
        if (training.Trainer != this)
            throw new AnotherTrainerEditTrainingException(training, this);
        if (!_trainings.Contains(training))
            throw new TrainingNotBelongTrainerException(training, this);

        return training.Complete(this);
    }

    private bool HasTrainingAt(TrainingTime time, Training? excludeTraining = null)
        => _trainings.Any(t => t != excludeTraining && t.Time.Overlaps(time));
}