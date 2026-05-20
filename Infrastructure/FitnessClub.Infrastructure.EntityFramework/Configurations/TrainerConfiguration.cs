using Domain.FitnessClub.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessClub.Infrastructure.EntityFramework.Configurations
{
    public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
    {
        public void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.Username)
                .IsRequired()
                .HasConversion(username => username.Value, str => new Username(str))
                .HasMaxLength(20);

            builder.HasMany<Training>("_trainings")
                .WithOne(x => x.Trainer)
                .HasForeignKey("TrainerId")
                .HasPrincipalKey(x => x.Id);

            builder.Ignore(x => x.Trainings);
        }
    }
}