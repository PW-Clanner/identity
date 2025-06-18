using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pw.Clanner.Identity.Domain.Entities;

namespace Pw.Clanner.Identity.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
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