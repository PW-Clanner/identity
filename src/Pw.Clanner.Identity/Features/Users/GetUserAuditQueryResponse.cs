using Pw.Clanner.Identity.Domain.Entities.Users;

namespace Pw.Clanner.Identity.Features.Users;

public record GetUserAuditQueryResponse(DateTime CreatedAt, UserAuditType Type);