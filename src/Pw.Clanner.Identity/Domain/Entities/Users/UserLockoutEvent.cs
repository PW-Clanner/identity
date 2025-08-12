using Pw.Clanner.Identity.Common;

namespace Pw.Clanner.Identity.Domain.Entities.Users;

internal sealed class UserLockoutEvent(User user, DateTime lockoutUntil, string lockoutReason) : DomainEvent
{
    public User User { get; } = user;
    public DateTime LockoutUntil { get; } = lockoutUntil;
    public string LockoutReason { get; } = lockoutReason;
}