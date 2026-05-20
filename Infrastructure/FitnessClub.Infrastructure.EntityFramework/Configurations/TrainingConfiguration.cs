using Domain.FitnessClub.Entities;
using Domain.FitnessClub.Enums;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessClub.Infrastructure.EntityFramework.Configurations
{
    public class TrainingConfiguration : IEntityTypeConfiguration<Training>
    {
        public void Configure(EntityTypeBuilder<Training> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasConversion(title => title.Value, str => new TrainingTitle(str))
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasConversion(description => description.Value, str => new Description(str))
                .HasMaxLength(255);

            builder.OwnsOne(x => x.Time, timeBuilder =>
            {
                timeBuilder.Property(t => t.StartTime)
                    .IsRequired()
                    .HasColumnName("DateTime")
                    .HasConversion(
                        src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                        dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc));

                timeBuilder.Property(t => t.DurationMinutes)
                    .IsRequired()
                    .HasColumnName("DurationMinutes");
            });

            builder.Property(x => x.MaxParticipants).IsRequired();
            builder.Property(x => x.AvailablePlaces).IsRequired();
            builder.Property(x => x.Room).HasMaxLength(50);
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasConversion(
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc));

            builder.Property(x => x.LastModifiedAt)
                .HasConversion(
                    src => src.HasValue && src.Value.Kind != DateTimeKind.Utc ? DateTime.SpecifyKind(src.Value, DateTimeKind.Utc) : src,
                    dst => dst.HasValue && dst.Value.Kind != DateTimeKind.Utc ? DateTime.SpecifyKind(dst.Value, DateTimeKind.Utc) : dst);

            builder.HasOne(x => x.Trainer)
                .WithMany("_trainings")
                .HasForeignKey("TrainerId")
                .HasPrincipalKey(x => x.Id);

            builder.HasMany<Registration>("_registrations")
                .WithOne(x => x.Training)
                .HasForeignKey("TrainingId")
                .HasPrincipalKey(x => x.Id);

            builder.Ignore(x => x.IsActive);
            builder.Ignore(x => x.Registrations);
            builder.Ignore(x => x.ConfirmedRegistrationsCount);
            builder.Ignore(x => x.HasAvailablePlaces);
        }
    }
}