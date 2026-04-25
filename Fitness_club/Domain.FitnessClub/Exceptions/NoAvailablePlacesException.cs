using Domain.FitnessClub.Entities;

namespace Domain.FitnessClub.Exceptions;

public class NoAvailablePlacesException(Training training) : InvalidOperationException($"No available places for training {training.Title.Value} (id = {training.Id})")
{
    public Training Training => training;
}