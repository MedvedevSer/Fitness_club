using Domain.FitnessClub.Entities;

namespace Domain.FitnessClub.Exceptions;

/// <summary>
/// Исключение: клиент уже записан на тренировку
/// </summary>
public class ClientAlreadyRegisteredException : InvalidOperationException
{
    public Client Client { get; }
    public Training Training { get; }

    public ClientAlreadyRegisteredException(Client client, Training training)
        : base($"Client {client.Username} is already registered for training '{training.Title}' (id = {training.Id}).")
    {
        Client = client;
        Training = training;
    }
}