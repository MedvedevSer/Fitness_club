using Domain.FitnessClub.Entities;
using Domain.FitnessClub.Enums;
using Domain.ValueObjects;
using FitnessClub.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FitnessClub.Infrastructure.EntityFramework.RepositoriesEF
{
    public class EfTrainerRepository(ApplicationDbContext context) : EfRepository<Trainer, Guid>(context)
    {
        public async Task<Trainer?> GetByUsernameAsync(Username username)
            => await context.Set<Trainer>()
                .FirstOrDefaultAsync(t => t.Username.Value == username.Value);

        public async Task<IEnumerable<Training>> GetTrainerTrainingsAsync(Guid trainerId)
            => await context.Set<Training>()
                .Where(t => t.Trainer.Id == trainerId)
                .Include(t => t.Trainer)
                .ToListAsync();

        public async Task<IEnumerable<Training>> GetUpcomingTrainingsAsync(Guid trainerId)
            => await context.Set<Training>()
                .Where(t => t.Trainer.Id == trainerId && t.Time.StartTime > DateTime.UtcNow && t.Status == TrainingStatus.Scheduled)
                .OrderBy(t => t.Time.StartTime)
                .ToListAsync();
    }
}