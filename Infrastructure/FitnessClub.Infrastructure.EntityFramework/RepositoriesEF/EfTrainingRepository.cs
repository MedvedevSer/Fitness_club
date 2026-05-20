using Domain.FitnessClub.Entities;
using Domain.FitnessClub.Enums;
using FitnessClub.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FitnessClub.Infrastructure.EntityFramework.RepositoriesEF
{
    public class EfTrainingRepository(ApplicationDbContext context) : EfRepository<Training, Guid>(context)
    {
        public async Task<IEnumerable<Training>> GetByTrainerAsync(Guid trainerId)
            => await context.Set<Training>()
                .Where(t => t.Trainer.Id == trainerId)
                .Include(t => t.Trainer)
                .ToListAsync();

        public async Task<IEnumerable<Training>> GetByStatusAsync(TrainingStatus status)
            => await context.Set<Training>()
                .Where(t => t.Status == status)
                .Include(t => t.Trainer)
                .ToListAsync();

        public async Task<IEnumerable<Training>> GetUpcomingTrainingsAsync()
            => await context.Set<Training>()
                .Where(t => t.Time.StartTime > DateTime.UtcNow && t.Status == TrainingStatus.Scheduled)
                .OrderBy(t => t.Time.StartTime)
                .Include(t => t.Trainer)
                .ToListAsync();

        public async Task<IEnumerable<Training>> GetByClientAsync(Guid clientId)
            => await context.Set<Training>()
                .Where(t => t.Registrations.Any(r => r.Client.Id == clientId))
                .Include(t => t.Trainer)
                .ToListAsync();

        public async Task<int> GetAvailablePlacesAsync(Guid trainingId)
        {
            var training = await GetByIdAsync(trainingId);
            return training?.AvailablePlaces ?? 0;
        }
    }
}