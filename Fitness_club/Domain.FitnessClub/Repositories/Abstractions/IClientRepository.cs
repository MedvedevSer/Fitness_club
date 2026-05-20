using Domain.FitnessClub.Entities;
using Domain.FitnessClub.Enums;
using Domain.ValueObjects;

namespace Domain.FitnessClub.Repositories.Abstractions
{
    public interface IClientRepository : IRepository<Client, Guid>
    {
        Task<Client?> GetByUsernameAsync(Username username);
        Task<IEnumerable<Client>> GetByRegistrationStatusAsync(RegistrationStatus status);
        Task<IEnumerable<Registration>> GetClientRegistrationsAsync(Guid clientId);
    }
}