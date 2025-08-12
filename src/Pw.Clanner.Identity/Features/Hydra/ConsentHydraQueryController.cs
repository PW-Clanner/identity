using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ory.Hydra.Client.Api;
using Ory.Hydra.Client.Model;
using Pw.Clanner.Identity.Common;
using Pw.Clanner.Identity.Common.Interfaces;
using Pw.Clanner.Identity.Domain.Entities.Users;
using Pw.Clanner.Identity.Infrastructure.Persistence;

namespace Pw.Clanner.Identity.Features.Hydra;

public class ConsentHydraQueryController(
    ICurrentHydraChallenge hydraChallenge,
    IOAuth2ApiAsync oauth2Api,
    AppDbContext dbContext)
    : ApiControllerBase
{
    [HttpGet("/hydra/consent")]
    public async Task<ActionResult> ConsentHydra(ConsentHydraQuery query, CancellationToken cancellationToken)
    {
        hydraChallenge.ConsentChallenge = query.ConsentChallenge;
        var consentRequest =
            await oauth2Api.GetOAuth2ConsentRequestAsync(query.ConsentChallenge, cancellationToken: cancellationToken);

        var acceptResponse = await oauth2Api.AcceptOAuth2ConsentRequestAsync(query.ConsentChallenge,
            new HydraAcceptOAuth2ConsentRequest(grantScope: consentRequest.RequestedScope,
                grantAccessTokenAudience: consentRequest.RequestedAccessTokenAudience),
            cancellationToken: cancellationToken);
        
        var user = await dbContext.Users.SingleOrDefaultAsync(x => x.Id == consentRequest.Subject,
            cancellationToken);
        user.DomainEvents.Add(new UserAuthorizedEvent(user));
        await dbContext.SaveChangesAsync(cancellationToken);

        return Redirect(acceptResponse.RedirectTo);
    }
}