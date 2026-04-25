using Domain.FitnessClub.Base;
using Domain.FitnessClub.Exceptions;
using Domain.ValueObjects;

namespace Domain.FitnessClub.Entities;

public class Client(Guid id, Username username) : Entity<Guid>(id)
{
    private readonly ICollection<Registration> _registrations = [];

    public Username Username { get; private set; } = username ?? throw new ArgumentNullException(nameof(username));

    public IReadOnlyCollection<Registration> Registrations => _registrations.ToList().AsReadOnly();

    protected Client() : this(Guid.NewGuid(), null!) { }

    public Client(Username username) : this(Guid.NewGuid(), username) { }

    public bool ChangeUsername(Username newUsername)
    {
        if (Username == newUsername) return false;
        Username = newUsername;
        return true;
    }

    public Registration RegisterForTraining(Training training)
    {
        var registration = training.RegisterClient(this);
        _registrations.Add(registration);
        return registration;
    }

    public bool CancelRegistration(Registration registration)
    {
        if (registration.Client != this) return false;
        var result = registration.Cancel();
        if (result) _registrations.Remove(registration);
        return result;
    }

    public bool IsRegisteredForTraining(Guid trainingId)
        => _registrations.Any(r => r.TrainingId == trainingId && r.IsActive);
}