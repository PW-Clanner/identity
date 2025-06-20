using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pw.Clanner.Identity.Domain.Entities.Users;

namespace Pw.Clanner.Identity.Infrastructure.Persistence.Configurations;

public class UserAuditConfiguration : IEntityTypeConfiguration<UserAudit>
{
    public void Configure(EntityTypeBuilder<UserAudit> builder)
    {
        builder.Property(x => x.Type)
            .IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Audits);
    }
}