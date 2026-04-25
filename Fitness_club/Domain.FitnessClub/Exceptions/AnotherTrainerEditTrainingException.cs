using Domain.FitnessClub.Entities;

namespace Domain.FitnessClub.Exceptions;

public class AnotherTrainerEditTrainingException(Training training, Trainer trainer)
    : InvalidOperationException($"Trainer {trainer.Username.Value} cannot edit training {training.Title.Value} created by {training.Trainer?.Username.Value} (training id = {training.Id})")
{
    public Training Training => training;
    public Trainer Trainer => trainer;
}