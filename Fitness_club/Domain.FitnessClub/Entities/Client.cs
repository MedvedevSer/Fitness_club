using Domain.FitnessClub.Base;
using Domain.FitnessClub.Exceptions;
using Domain.ValueObjects;

namespace Domain.FitnessClub.Entities;

public class Client : Entity<Guid>
{
    private readonly ICollection<Registration> _registrations = [];

    public Username Username { get; private set; }

    public IReadOnlyCollection<Registration> Registrations => _registrations.ToList().AsReadOnly();

    protected Client() { }

    public Client(Guid id, Username username) : base(id)
    {
        Username = username ?? throw new ArgumentNullValueException(nameof(username));
    }

    public Client(Username username) : this(Guid.NewGuid(), username) { }

    public bool ChangeUsername(Username newUsername)
    {
        if (Username == newUsername) return false;
        Username = newUsername;
        return true;
    }

    public Registration RegisterForTraining(Training training)
    {
        if (IsRegisteredForTraining(training))
            throw new ClientAlreadyRegisteredException(this, training);

        var registration = training.RegisterClient(this);
        _registrations.Add(registration);
        return registration;
    }

    public bool CancelRegistration(Registration registration, Trainer trainer)
    {
        if (registration.Client != this) return false;
        if (!_registrations.Contains(registration)) return false;

        var result = registration.Cancel(trainer);
        if (result) _registrations.Remove(registration);
        return result;
    }

    public bool IsRegisteredForTraining(Training training)
        => _registrations.Any(r => r.Training == training && r.IsActive);
}