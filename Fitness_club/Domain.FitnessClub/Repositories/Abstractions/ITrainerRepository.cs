using Domain.FitnessClub.Entities;
using Domain.ValueObjects;

namespace Domain.FitnessClub.Repositories.Abstractions
{
    public interface ITrainerRepository : IRepository<Trainer, Guid>
    {
        Task<Trainer?> GetByUsernameAsync(Username username);
        Task<IEnumerable<Training>> GetTrainerTrainingsAsync(Guid trainerId);
        Task<IEnumerable<Training>> GetUpcomingTrainingsAsync(Guid trainerId);
    }
}