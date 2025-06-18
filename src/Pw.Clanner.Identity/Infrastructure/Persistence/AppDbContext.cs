using Microsoft.EntityFrameworkCore;
using Pw.Clanner.Identity.Domain.Entities;

namespace Pw.Clanner.Identity.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
}