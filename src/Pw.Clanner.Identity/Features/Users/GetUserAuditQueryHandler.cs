using MediatR;
using Pw.Clanner.Identity.Common.Mappings;
using Pw.Clanner.Identity.Common.Models;
using Pw.Clanner.Identity.Domain.Entities.Users;
using Pw.Clanner.Identity.Infrastructure.Persistence;

namespace Pw.Clanner.Identity.Features.Users;

internal sealed class GetUserAuditQueryHandler(AppDbContext dbContext) : IRequestHandler<GetUserAuditQuery, PaginatedList<GetUserAuditQueryResponse>>
{
    public Task<PaginatedList<GetUserAuditQueryResponse>> Handle(GetUserAuditQuery request, CancellationToken cancellationToken)
    {
        return dbContext.UserAudits
            .Where(item => item.User.Id == request.UserId)
            .OrderBy(item => item.CreatedAt)
            .Select(item => ToDto(item))
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }

    private static GetUserAuditQueryResponse ToDto(UserAudit userAudit) =>
        new(userAudit.CreatedAt, userAudit.Type);
}