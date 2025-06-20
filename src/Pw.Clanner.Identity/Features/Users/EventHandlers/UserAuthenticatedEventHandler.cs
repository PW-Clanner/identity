using MediatR;
using Microsoft.Extensions.Logging;
using Pw.Clanner.Identity.Common.Models;
using Pw.Clanner.Identity.Domain.Entities.Users;
using Pw.Clanner.Identity.Infrastructure.Persistence;

namespace Pw.Clanner.Identity.Features.Users.EventHandlers;

internal sealed class UserAuthenticatedEventHandler(ILogger<UserAuthenticatedEventHandler> logger, AppDbContext dbContext)
    : INotificationHandler<DomainEventNotification<UserAuthenticatedEvent>>
{
    public async Task Handle(DomainEventNotification<UserAuthenticatedEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        logger.LogInformation("Обработка доменного события: {DomainEvent}", domainEvent.GetType().Name);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}