namespace Domain.FitnessClub.Exceptions;

/// <summary>
/// Исключение: нет свободных мест на тренировку
/// </summary>
public class NoAvailablePlacesException : InvalidOperationException
{
    public NoAvailablePlacesException()
        : base("No available places for this training.")
    {
    }
}