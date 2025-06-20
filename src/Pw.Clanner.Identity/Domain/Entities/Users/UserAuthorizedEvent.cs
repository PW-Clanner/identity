using Pw.Clanner.Identity.Common;

namespace Pw.Clanner.Identity.Domain.Entities.Users;

internal sealed class UserAuthorizedEvent(User user) : DomainEvent
{
    public User User { get; } = user;
}