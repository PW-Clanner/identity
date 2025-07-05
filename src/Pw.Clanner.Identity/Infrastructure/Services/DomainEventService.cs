using MediatR;
using Microsoft.Extensions.Logging;
using Pw.Clanner.Identity.Common;
using Pw.Clanner.Identity.Common.Interfaces;
using Pw.Clanner.Identity.Common.Models;

namespace Pw.Clanner.Identity.Infrastructure.Services;

public class DomainEventService(ILogger<DomainEventService> logger, IPublisher mediator) : IDomainEventService
{
    public Task Publish(DomainEvent domainEvent)
    {
        logger.LogInformation("Публикация доменного события. Событие - {event}", domainEvent.GetType().Name);
        return mediator.Publish(GetNotificationCorrespondingToDomainEvent(domainEvent));
    }

    private static INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
    {
        return (INotification)Activator.CreateInstance(
            typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent)!;
    }
}