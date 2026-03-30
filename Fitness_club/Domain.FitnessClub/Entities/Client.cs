using Domain.FitnessClub.Base;
using Domain.FitnessClub.Exceptions;
using Domain.ValueObjects;

namespace Domain.FitnessClub.Entities;

public class Client : Entity<Guid>
{
    public Username Username { get; private set; }

    private readonly List<Registration> _registrations = new();
    public IReadOnlyCollection<Registration> Registrations => _registrations.AsReadOnly();

    protected Client()
    {
        Username = null!;
    }

    public Client(Username username) : this(Guid.NewGuid(), username)
    {
    }

    public Client(Guid id, Username username) : base(id)
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

    public Registration RegisterForTraining(Training training)
    {
        if (training == null) throw new ArgumentNullException(nameof(training));

        var registration = training.RegisterClient(this);
        _registrations.Add(registration);
        return registration;
    }

    public bool CancelRegistration(Registration registration)
    {
        if (registration == null) throw new ArgumentNullException(nameof(registration));
        if (registration.Client != this)
            throw new InvalidOperationException("Registration does not belong to this client.");

        var result = registration.Cancel();
        if (result)
            _registrations.Remove(registration);
        return result;
    }

    public bool IsRegisteredForTraining(Guid trainingId)
    {
        return _registrations.Any(r => r.TrainingId == trainingId && r.IsActive);
    }
}