using Domain.FitnessClub.Entities;
using Domain.FitnessClub.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessClub.Infrastructure.EntityFramework.Configurations
{
    public class RegistrationConfiguration : IEntityTypeConfiguration<Registration>
    {
        public void Configure(EntityTypeBuilder<Registration> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.RegistrationDate)
                .IsRequired()
                .HasConversion(
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc));

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasDefaultValue(RegistrationStatus.Confirmed);

            builder.HasOne(x => x.Training)
                .WithMany("_registrations")
                .HasForeignKey("TrainingId")
                .HasPrincipalKey(x => x.Id);

            builder.HasOne(x => x.Client)
                .WithMany("_registrations")
                .HasForeignKey("ClientId")
                .HasPrincipalKey(x => x.Id);

            builder.Navigation(x => x.Training).AutoInclude();
            builder.Navigation(x => x.Client).AutoInclude();

            builder.Ignore(x => x.IsActive);
        }
    }
}