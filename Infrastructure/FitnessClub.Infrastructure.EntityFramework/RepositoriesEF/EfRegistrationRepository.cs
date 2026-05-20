using Domain.FitnessClub.Entities;
using Domain.FitnessClub.Enums;
using FitnessClub.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FitnessClub.Infrastructure.EntityFramework.RepositoriesEF
{
    public class EfRegistrationRepository(ApplicationDbContext context) : EfRepository<Registration, Guid>(context)
    {
        public async Task<IEnumerable<Registration>> GetByClientAsync(Guid clientId)
            => await context.Set<Registration>()
                .Where(r => r.Client.Id == clientId)
                .Include(r => r.Training)
                .ThenInclude(t => t.Trainer)
                .ToListAsync();

        public async Task<IEnumerable<Registration>> GetByTrainingAsync(Guid trainingId)
            => await context.Set<Registration>()
                .Where(r => r.Training.Id == trainingId)
                .Include(r => r.Client)
                .ToListAsync();

        public async Task<IEnumerable<Registration>> GetByStatusAsync(RegistrationStatus status)
            => await context.Set<Registration>()
                .Where(r => r.Status == status)
                .Include(r => r.Client)
                .Include(r => r.Training)
                .ToListAsync();

        public async Task<Registration?> GetByClientAndTrainingAsync(Guid clientId, Guid trainingId)
            => await context.Set<Registration>()
                .FirstOrDefaultAsync(r => r.Client.Id == clientId && r.Training.Id == trainingId);

        public async Task<bool> IsClientRegisteredAsync(Guid clientId, Guid trainingId)
            => await context.Set<Registration>()
                .AnyAsync(r => r.Client.Id == clientId && r.Training.Id == trainingId && r.IsActive);
    }
}