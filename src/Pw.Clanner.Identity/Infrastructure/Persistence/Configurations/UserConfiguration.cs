using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pw.Clanner.Identity.Domain.Entities.Users;

namespace Pw.Clanner.Identity.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Ignore(x => x.DomainEvents);

        builder.Property(x => x.UserName)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(x => x.Email)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(x => x.PasswordHash)
            .IsRequired();

        builder.HasIndex(x => x.UserName)
            .IsUnique();
        builder.HasIndex(x => x.Email)
            .IsUnique();
    }
}