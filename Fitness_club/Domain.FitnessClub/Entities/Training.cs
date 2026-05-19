using Domain.FitnessClub.Base;
using Domain.FitnessClub.Enums;
using Domain.FitnessClub.Exceptions;
using Domain.ValueObjects;

namespace Domain.FitnessClub.Entities;

public class Training : Entity<Guid>
{
    private readonly ICollection<Registration> _registrations = [];

    public TrainingTitle Title { get; private set; }
    public Description Description { get; private set; }
    public TrainingTime Time { get; private set; }
    public int MaxParticipants { get; private set; }
    public int AvailablePlaces { get; private set; }
    public string Room { get; private set; }
    public TrainingStatus Status { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? LastModifiedAt { get; private set; }
    public Trainer Trainer { get; }
    public bool IsActive => Status == TrainingStatus.Scheduled;
    public IReadOnlyCollection<Registration> Registrations => _registrations.ToList().AsReadOnly();
    public int ConfirmedRegistrationsCount => _registrations.Count(r => r.IsActive);
    public bool HasAvailablePlaces => AvailablePlaces > 0;

    protected Training() { }

    public Training(Trainer trainer, TrainingTitle title, Description description, TrainingTime time, int maxParticipants, string room)
        : this(Guid.NewGuid(), trainer, title, description, time, maxParticipants, room, DateTime.UtcNow, null) { }

    protected Training(Guid id, Trainer trainer, TrainingTitle title, Description description, TrainingTime time, int maxParticipants, string room, DateTime createdAt, DateTime? lastModifiedAt)
        : base(id)
    {
        Trainer = trainer ?? throw new ArgumentNullValueException(nameof(trainer));
        Title = title ?? throw new ArgumentNullValueException(nameof(title));
        Description = description ?? throw new ArgumentNullValueException(nameof(description));
        Time = time ?? throw new ArgumentNullValueException(nameof(time));
        MaxParticipants = maxParticipants;
        AvailablePlaces = maxParticipants;
        Room = room ?? throw new ArgumentNullValueException(nameof(room));
        Status = TrainingStatus.Scheduled;
        CreatedAt = createdAt;
        LastModifiedAt = lastModifiedAt;
    }

    public Registration RegisterClient(Client client)
    {
        if (client == null) throw new ArgumentNullValueException(nameof(client));
        if (Status != TrainingStatus.Scheduled)
            throw new InvalidOperationException("Cannot register for cancelled or completed training.");
        if (Time.IsPast)
            throw new InvalidOperationException("Cannot register for past training.");
        if (AvailablePlaces <= 0)
            throw new NoAvailablePlacesException(this);
        if (_registrations.Any(r => r.Client == client && r.IsActive))
            throw new ClientAlreadyRegisteredException(client, this);

        var registration = new Registration(this, client);
        _registrations.Add(registration);
        AvailablePlaces--;
        return registration;
    }

    public bool Cancel(Trainer trainer)
    {
        if (trainer != Trainer) return false;
        if (Status == TrainingStatus.Cancelled) return false;
        if (Time.IsPast) throw new CannotCancelPastTrainingException(this);

        Status = TrainingStatus.Cancelled;
        foreach (var reg in _registrations.Where(r => r.IsActive))
            reg.Cancel(trainer);
        AvailablePlaces = 0;
        return true;
    }

    public bool Complete(Trainer trainer)
    {
        if (trainer != Trainer) return false;
        if (!Time.IsPast) return false;
        if (Status == TrainingStatus.Completed) return false;

        Status = TrainingStatus.Completed;
        return true;
    }

    public bool UpdateDetails(Trainer trainer, TrainingTitle newTitle, Description newDescription, string newRoom)
    {
        if (trainer != Trainer) return false;

        var updated = false;
        if (Title != newTitle) { Title = newTitle; updated = true; }
        if (Description != newDescription) { Description = newDescription; updated = true; }
        if (Room != newRoom) { Room = newRoom; updated = true; }
        if (updated) LastModifiedAt = DateTime.UtcNow;
        return updated;
    }

    public bool Reschedule(Trainer trainer, TrainingTime newTime)
    {
        if (trainer != Trainer) return false;
        if (Time == newTime) return false;
        if (Status != TrainingStatus.Scheduled)
            throw new InvalidOperationException("Cannot reschedule cancelled or completed training.");

        Time = newTime;
        LastModifiedAt = DateTime.UtcNow;
        return true;
    }
}