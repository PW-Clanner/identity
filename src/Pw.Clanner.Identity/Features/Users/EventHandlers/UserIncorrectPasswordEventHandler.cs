using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pw.Clanner.Identity.Common.Models;
using Pw.Clanner.Identity.Domain.Entities.Users;
using Pw.Clanner.Identity.Infrastructure.Persistence;

namespace Pw.Clanner.Identity.Features.Users.EventHandlers;

internal sealed class UserIncorrectPasswordEventHandler(ILogger<UserIncorrectPasswordEventHandler> logger, AppDbContext dbContext)
    : INotificationHandler<DomainEventNotification<UserIncorrectPasswordEvent>>
{
    public async Task Handle(DomainEventNotification<UserIncorrectPasswordEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        logger.LogInformation("Обработка доменного события: {DomainEvent}", domainEvent.GetType().Name);

        var audit = new UserAudit
        {
            User = domainEvent.User,
            Type = UserAuditType.IncorrectPassword
        };

        await dbContext.UserAudits.AddAsync(audit, cancellationToken);

        var countEvents = await dbContext.UserAudits
            .Where(x => x.User == domainEvent.User)
            .Where(x => x.Type == UserAuditType.IncorrectPassword)
            .Where(x => x.CreatedAt >= DateTime.Now.AddMinutes(-5))
            .CountAsync(cancellationToken: cancellationToken);

        if (countEvents >= 5)
        {
            domainEvent.User.DomainEvents.Add(new UserLockoutEvent(domainEvent.User, DateTime.Now.AddHours(1),
                "Неверный пароль более 5 раз"));
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation("Добавлена запись аудита: {@Audit}", audit);
    }
}