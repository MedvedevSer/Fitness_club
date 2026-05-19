using Domain.FitnessClub.Entities;

namespace Domain.FitnessClub.Exceptions;

public class CannotCancelPastTrainingException(Training training)
    : InvalidOperationException($"Cannot cancel past training {training.Title.Value} (id = {training.Id})")
{
    public Training Training => training;
}