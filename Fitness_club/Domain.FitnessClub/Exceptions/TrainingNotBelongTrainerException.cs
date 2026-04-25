using Domain.FitnessClub.Entities;

namespace Domain.FitnessClub.Exceptions;

public class TrainingNotBelongTrainerException(Training training, Trainer trainer)
    : InvalidOperationException($"Training {training.Title.Value} does not belong to trainer {trainer.Username.Value} (training id = {training.Id})")
{
    public Training Training => training;
    public Trainer Trainer => trainer;
}