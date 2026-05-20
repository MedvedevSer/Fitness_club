using Domain.FitnessClub.Entities;
using Domain.FitnessClub.Enums;

namespace Domain.FitnessClub.Repositories.Abstractions
{
    public interface ITrainingRepository : IRepository<Training, Guid>
    {
        Task<IEnumerable<Training>> GetByTrainerAsync(Guid trainerId);
        Task<IEnumerable<Training>> GetByStatusAsync(TrainingStatus status);
        Task<IEnumerable<Training>> GetUpcomingTrainingsAsync();
        Task<IEnumerable<Training>> GetByClientAsync(Guid clientId);
        Task<int> GetAvailablePlacesAsync(Guid trainingId);
    }
}