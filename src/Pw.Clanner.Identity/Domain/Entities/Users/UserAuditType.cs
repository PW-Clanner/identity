namespace Pw.Clanner.Identity.Domain.Entities.Users;

public enum UserAuditType
{
    Register,
    Login,
    LoggedIn,
    IncorrectPassword,
    Lockout
}