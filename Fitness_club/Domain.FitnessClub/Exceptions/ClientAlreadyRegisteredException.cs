using Domain.FitnessClub.Entities;

namespace Domain.FitnessClub.Exceptions;

public class ClientAlreadyRegisteredException(Client client, Training training) : InvalidOperationException($"Client {client.Username.Value} already registered for training {training.Title.Value} (training id = {training.Id})")
{
    public Client Client => client;
    public Training Training => training;
}