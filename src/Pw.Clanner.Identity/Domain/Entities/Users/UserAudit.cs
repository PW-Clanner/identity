using Pw.Clanner.Identity.Domain.Entities.Abstract;

namespace Pw.Clanner.Identity.Domain.Entities.Users;

public class UserAudit : EntityBase
{
    public User User { get; set; }
    
    public UserAuditType Type { get; set; }
}