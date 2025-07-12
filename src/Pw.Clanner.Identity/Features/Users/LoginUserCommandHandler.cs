using MediatR;
using Microsoft.EntityFrameworkCore;
using Ory.Hydra.Client.Api;
using Ory.Hydra.Client.Model;
using Pw.Clanner.Identity.Common;
using Pw.Clanner.Identity.Common.Interfaces;
using Pw.Clanner.Identity.Domain.Entities.Users;
using Pw.Clanner.Identity.Infrastructure.Persistence;

namespace Pw.Clanner.Identity.Features.Users;

internal sealed class LoginUserCommandHandler(
    ICurrentHydraChallenge hydraChallenge,
    IOAuth2ApiAsync oauth2ApiAsync,
    AppDbContext dbContext)
    : IRequestHandler<LoginUserCommand, LoginUserCommandResponse>
{
    public async Task<LoginUserCommandResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var userName = request.UserName.ToLower();
        var user = await dbContext.Users
            .Where(x => x.UserName.ToLower() == userName)
            .SingleOrDefaultAsync(cancellationToken);

        if (user is null)
            return new LoginUserCommandResponse(false, null);

        user.DomainEvents.Add(new UserIdentifiedEvent(user));

        var result = user.VerifyPasswordHash(request.Password);

        user.DomainEvents.Add(result ? new UserAuthenticatedEvent(user) : new UserIncorrectPasswordEvent(user));

        if (result)
        {
            user.DomainEvents.Add(new UserAuthorizedEvent(user));
            return new LoginUserCommandResponse(true, user.Id);
        }

        await dbContext.SaveChangesAsync(cancellationToken);


        return new LoginUserCommandResponse(false, null);
    }
}