namespace Domain.FitnessClub.Exceptions;

/// <summary>
/// Исключение: некорректное время тренировки
/// </summary>
public class InvalidTrainingTimeException : ArgumentException
{
    public InvalidTrainingTimeException(string message)
        : base(message)
    {
    }
}