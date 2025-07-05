using MediatR;
using Pw.Clanner.Identity.Common.Models;

namespace Pw.Clanner.Identity.Features.Users;

public record GetUserAuditQuery(string UserId, int PageNumber = 1, int PageSize = 10)
    : IRequest<PaginatedList<GetUserAuditQueryResponse>>;