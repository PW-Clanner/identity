using Microsoft.AspNetCore.Mvc;
using Pw.Clanner.Identity.Common;
using Pw.Clanner.Identity.Common.Models;

namespace Pw.Clanner.Identity.Features.Users;

public class GetUserAuditQueryController : ApiControllerBase
{
    [HttpGet("/api/users/audit")]
    public async Task<ActionResult<PaginatedList<GetUserAuditQueryResponse>>> GetUserAudit([FromQuery]GetUserAuditQuery query)
    {
        return await Mediatr.Send(query);
    }
}