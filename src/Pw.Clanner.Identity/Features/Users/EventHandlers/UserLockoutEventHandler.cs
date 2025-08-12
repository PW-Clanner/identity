using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pw.Clanner.Identity.Common.Models;
using Pw.Clanner.Identity.Domain.Entities.Users;
using Pw.Clanner.Identity.Infrastructure.Persistence;

namespace Pw.Clanner.Identity.Features.Users.EventHandlers;

internal sealed class UserLockoutEventHandler(ILogger<UserLockoutEventHandler> logger, AppDbContext dbContext)
    : INotificationHandler<DomainEventNotification<UserLockoutEvent>>
{
    public async Task Handle(DomainEventNotification<UserLockoutEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        logger.LogInformation("Обработка доменного события: {DomainEvent}", domainEvent.GetType().Name);

        var audit = new UserAudit
        {
            User = domainEvent.User,
            Type = UserAuditType.Lockout
        };

        await dbContext.UserAudits.AddAsync(audit, cancellationToken);
        domainEvent.User.LockoutUntil = domainEvent.LockoutUntil;
        domainEvent.User.LockoutReason = domainEvent.LockoutReason;
        dbContext.Update(domainEvent.User);

        await dbContext.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation("Добавлена запись аудита: {@Audit}", audit);
    }
}