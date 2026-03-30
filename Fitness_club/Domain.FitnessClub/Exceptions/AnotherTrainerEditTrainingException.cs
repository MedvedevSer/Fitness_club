using Domain.FitnessClub.Entities;

namespace Domain.FitnessClub.Exceptions;

/// <summary>
/// Исключение: тренер пытается редактировать чужую тренировку
/// </summary>
public class AnotherTrainerEditTrainingException : InvalidOperationException
{
    public Training Training { get; }
    public Trainer Trainer { get; }

    public AnotherTrainerEditTrainingException(Training training, Trainer trainer)
        : base($"The trainer {trainer.Username} can't edit the training '{training.Title}' owned by the trainer {training.Trainer?.Username} (training id = {training.Id}).")
    {
        Training = training;
        Trainer = trainer;
    }
}