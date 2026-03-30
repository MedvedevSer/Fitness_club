using Domain.FitnessClub.Base;
using Domain.FitnessClub.Enums;

namespace Domain.FitnessClub.Entities;

public class Registration : Entity<Guid>
{
    public Guid TrainingId { get; private set; }
    public Training? Training { get; private set; }

    public Guid ClientId { get; private set; }
    public Client? Client { get; private set; }

    public DateTime RegistrationDate { get; private set; }
    public RegistrationStatus Status { get; private set; }

    protected Registration() { }

    public Registration(Training training, Client client)
        : this(Guid.NewGuid(), training, client, DateTime.UtcNow, RegistrationStatus.Confirmed)
    {
    }

    public Registration(
        Guid id,
        Training training,
        Client client,
        DateTime registrationDate,
        RegistrationStatus status)
        : base(id)
    {
        Training = training ?? throw new ArgumentNullException(nameof(training));
        TrainingId = training.Id;
        Client = client ?? throw new ArgumentNullException(nameof(client));
        ClientId = client.Id;
        RegistrationDate = registrationDate;
        Status = status;
    }

    public bool Cancel()
    {
        if (Status == RegistrationStatus.Cancelled) return false;
        if (Status == RegistrationStatus.Attended)
            throw new InvalidOperationException("Cannot cancel attended registration.");
        if (Training!.Time.IsPast)
            throw new InvalidOperationException("Cannot cancel past training registration.");

        Status = RegistrationStatus.Cancelled;
        return true;
    }

    public bool MarkAttended()
    {
        if (Status != RegistrationStatus.Confirmed) return false;
        if (!Training!.Time.IsPast)
            throw new InvalidOperationException("Cannot mark future training as attended.");

        Status = RegistrationStatus.Attended;
        return true;
    }

    public bool IsActive => Status == RegistrationStatus.Confirmed;
    public bool IsAttended => Status == RegistrationStatus.Attended;
    public bool IsCancelled => Status == RegistrationStatus.Cancelled;
}