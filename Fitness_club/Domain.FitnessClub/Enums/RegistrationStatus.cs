namespace Domain.FitnessClub.Enums;

/// <summary>
/// Статус записи клиента на тренировку
/// </summary>
public enum RegistrationStatus
{
    /// <summary>Подтверждённая запись</summary>
    Confirmed,

    /// <summary>Отменённая запись</summary>
    Cancelled,

    /// <summary>Посещённая тренировка</summary>
    Attended
}