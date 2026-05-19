using Domain.FitnessClub.Entities;
using Domain.ValueObjects;

namespace Domain.FitnessClub.Exceptions;

public class TrainerAlreadyHasTrainingAtTimeException(Trainer trainer, TrainingTime time)
    : InvalidOperationException($"Trainer {trainer.Username.Value} already has a training at time {time.StartTime}")
{
    public Trainer Trainer => trainer;
    public TrainingTime Time => time;
}