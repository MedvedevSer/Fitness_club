using Domain.FitnessClub.Entities;
using Domain.FitnessClub.Enums;
using Domain.ValueObjects;
using FitnessClub.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FitnessClub.Infrastructure.EntityFramework.RepositoriesEF
{
    public class EfClientRepository(ApplicationDbContext context) : EfRepository<Client, Guid>(context)
    {
        public async Task<Client?> GetByUsernameAsync(Username username)
            => await context.Set<Client>()
                .FirstOrDefaultAsync(c => c.Username.Value == username.Value);

        public async Task<IEnumerable<Client>> GetByRegistrationStatusAsync(RegistrationStatus status)
            => await context.Set<Client>()
                .Where(c => c.Registrations.Any(r => r.Status == status))
                .ToListAsync();

        public async Task<IEnumerable<Registration>> GetClientRegistrationsAsync(Guid clientId)
            => await context.Set<Registration>()
                .Where(r => r.Client.Id == clientId)
                .Include(r => r.Training)
                .ToListAsync();
    }
}