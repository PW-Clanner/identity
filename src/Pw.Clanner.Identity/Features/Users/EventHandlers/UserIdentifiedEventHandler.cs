﻿using MediatR;
using Microsoft.Extensions.Logging;
using Pw.Clanner.Identity.Common.Models;
using Pw.Clanner.Identity.Domain.Entities.Users;
using Pw.Clanner.Identity.Infrastructure.Persistence;

namespace Pw.Clanner.Identity.Features.Users.EventHandlers;

internal sealed class UserIdentifiedEventHandler(ILogger<UserIdentifiedEventHandler> logger, AppDbContext dbContext)
    : INotificationHandler<DomainEventNotification<UserIdentifiedEvent>>
{
    public async Task Handle(DomainEventNotification<UserIdentifiedEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        logger.LogInformation("Обработка доменного события: {DomainEvent}", domainEvent.GetType().Name);

        var audit = new UserAudit
        {
            User = domainEvent.User,
            Type = UserAuditType.Login
        };

        await dbContext.UserAudits.AddAsync(audit, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation("Добавлена запись аудита: {@Audit}", audit);
    }
}