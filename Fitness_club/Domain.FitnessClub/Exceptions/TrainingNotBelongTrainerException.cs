using Domain.FitnessClub.Entities;

namespace Domain.FitnessClub.Exceptions;

/// <summary>
/// Исключение: тренировка не принадлежит тренеру
/// </summary>
public class TrainingNotBelongTrainerException : InvalidOperationException
{
    public Training Training { get; }
    public Trainer Trainer { get; }

    public TrainingNotBelongTrainerException(Training training, Trainer trainer)
        : base($"The training '{training.Title}' is not in the trainer's training sequence (trainer {trainer.Username}, training id = {training.Id}).")
    {
        Training = training;
        Trainer = trainer;
    }
}