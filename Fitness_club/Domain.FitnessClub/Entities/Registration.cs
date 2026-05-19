using Domain.FitnessClub.Base;
using Domain.FitnessClub.Enums;
using Domain.FitnessClub.Exceptions;

namespace Domain.FitnessClub.Entities;

public class Registration : Entity<Guid>
{
    public Training Training { get; }
    public Client Client { get; }
    public DateTime RegistrationDate { get; }
    public RegistrationStatus Status { get; private set; }

    public bool IsActive => Status == RegistrationStatus.Confirmed;

    protected Registration() { }

    public Registration(Training training, Client client)
        : this(Guid.NewGuid(), training, client, DateTime.UtcNow, RegistrationStatus.Confirmed) { }

    protected Registration(Guid id, Training training, Client client, DateTime registrationDate, RegistrationStatus status)
        : base(id)
    {
        Training = training ?? throw new ArgumentNullValueException(nameof(training));
        Client = client ?? throw new ArgumentNullValueException(nameof(client));
        RegistrationDate = registrationDate;
        Status = status;
    }

    public bool Cancel(Trainer trainer)
    {
        if (trainer != Training.Trainer) return false;
        if (Status == RegistrationStatus.Cancelled) return false;
        if (Status == RegistrationStatus.Attended) throw new InvalidOperationException("Cannot cancel attended registration.");
        if (Training.Time.IsPast) throw new InvalidOperationException("Cannot cancel past training registration.");

        Status = RegistrationStatus.Cancelled;
        return true;
    }

    public bool MarkAttended(Trainer trainer)
    {
        if (trainer != Training.Trainer) return false;
        if (Status != RegistrationStatus.Confirmed) return false;
        if (!Training.Time.IsPast) throw new InvalidOperationException("Cannot mark future training as attended.");

        Status = RegistrationStatus.Attended;
        return true;
    }
}