using Domain.FitnessClub.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessClub.Infrastructure.EntityFramework.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.Username)
                .IsRequired()
                .HasConversion(username => username.Value, str => new Username(str))
                .HasMaxLength(20);

            builder.HasMany<Registration>("_registrations")
                .WithOne(x => x.Client)
                .HasForeignKey("ClientId")
                .HasPrincipalKey(x => x.Id);

            builder.Ignore(x => x.Registrations);
        }
    }
}