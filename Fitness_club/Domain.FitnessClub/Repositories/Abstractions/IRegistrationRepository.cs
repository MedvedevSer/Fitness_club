using Domain.FitnessClub.Entities;
using Domain.FitnessClub.Enums;

namespace Domain.FitnessClub.Repositories.Abstractions
{
    public interface IRegistrationRepository : IRepository<Registration, Guid>
    {
        Task<IEnumerable<Registration>> GetByClientAsync(Guid clientId);
        Task<IEnumerable<Registration>> GetByTrainingAsync(Guid trainingId);
        Task<IEnumerable<Registration>> GetByStatusAsync(RegistrationStatus status);
        Task<Registration?> GetByClientAndTrainingAsync(Guid clientId, Guid trainingId);
        Task<bool> IsClientRegisteredAsync(Guid clientId, Guid trainingId);
    }
}