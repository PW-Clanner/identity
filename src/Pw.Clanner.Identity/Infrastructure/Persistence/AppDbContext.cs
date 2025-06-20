using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Pw.Clanner.Identity.Common;
using Pw.Clanner.Identity.Common.Interfaces;
using Pw.Clanner.Identity.Domain.Entities.Abstract;
using Pw.Clanner.Identity.Domain.Entities.Users;

namespace Pw.Clanner.Identity.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options, IDomainEventService domainEventService)
    : DbContext(options)
{
    private readonly IDomainEventService _domainEventService = domainEventService;
    
    public DbSet<User> Users { get; set; }

    public DbSet<UserAudit> UserAudits { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<EntityBase>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedAt = DateTime.Now;
                    break;
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    break;
                default:
                    break;
            }
        }

        var events = ChangeTracker.Entries<IHasDomainEvent>()
            .Select(x => x.Entity.DomainEvents)
            .SelectMany(x => x)
            .Where(domainEvent => !domainEvent.IsPublished)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        await DispatchEvents(events);

        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    private async Task DispatchEvents(List<DomainEvent> events)
    {
        foreach (var @event in events)
        {
            @event.IsPublished = true;
            await _domainEventService.Publish(@event);
        }
    }
}