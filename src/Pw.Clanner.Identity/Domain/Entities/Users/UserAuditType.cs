namespace Pw.Clanner.Identity.Domain.Entities.Users;

public enum UserAuditType
{
    Register,
    Identified,
    Authenticated,
    IncorrectPassword,
    Authorized,
    Lockout
}