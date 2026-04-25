using Domain.FitnessClub.Base;
using Domain.FitnessClub.Enums;
using Domain.FitnessClub.Exceptions;

namespace Domain.FitnessClub.Entities;

public class Registration : Entity<Guid>
{
    public Guid TrainingId { get; }
    public Training? Training { get; }
    public Guid ClientId { get; }
    public Client? Client { get; }
    public DateTime RegistrationDate { get; }
    public RegistrationStatus Status { get; private set; }
    public bool IsActive => Status == RegistrationStatus.Confirmed;
    public bool IsAttended => Status == RegistrationStatus.Attended;
    public bool IsCancelled => Status == RegistrationStatus.Cancelled;

    protected Registration() : base(Guid.NewGuid()) { }

    public Registration(Guid id, Training training, Client client, DateTime registrationDate, RegistrationStatus status)
        : base(id)
    {
        Training = training ?? throw new ArgumentNullValueException(nameof(training));
        TrainingId = training.Id;
        Client = client ?? throw new ArgumentNullValueException(nameof(client));
        ClientId = client.Id;
        RegistrationDate = registrationDate;
        Status = status;
    }

    public Registration(Training training, Client client)
        : this(Guid.NewGuid(), training, client, DateTime.UtcNow, RegistrationStatus.Confirmed) { }

    public bool Cancel()
    {
        if (Status == RegistrationStatus.Cancelled) return false;

        return Status switch
        {
            RegistrationStatus.Attended => throw new InvalidOperationException("Cannot cancel attended registration."),
            RegistrationStatus.Confirmed => CancelConfirmedRegistration(),
            _ => false
        };
    }

    public bool MarkAttended()
    {
        if (Status != RegistrationStatus.Confirmed) return false;
        if (!Training!.Time.IsPast)
            throw new InvalidOperationException("Cannot mark future training as attended.");

        Status = RegistrationStatus.Attended;
        return true;
    }

    private bool CancelConfirmedRegistration()
    {
        if (Training!.Time.IsPast)
            throw new InvalidOperationException("Cannot cancel past training registration.");

        Status = RegistrationStatus.Cancelled;
        return true;
    }
}