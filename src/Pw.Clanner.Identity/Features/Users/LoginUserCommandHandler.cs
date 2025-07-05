using MediatR;
using Microsoft.EntityFrameworkCore;
using Pw.Clanner.Identity.Common;
using Pw.Clanner.Identity.Domain.Entities.Users;
using Pw.Clanner.Identity.Infrastructure.Persistence;

namespace Pw.Clanner.Identity.Features.Users;

internal sealed class LoginUserCommandHandler(AppDbContext dbContext) : IRequestHandler<LoginUserCommand, string>
{
    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var userName = request.UserName.ToLower();
        var user = await dbContext.Users
            .Where(x => x.UserName.ToLower() == userName)
            .SingleOrDefaultAsync(cancellationToken);

        if (user is null)
            return null;


        user.DomainEvents.Add(new UserIdentifiedEvent(user));

        var result = user.VerifyPasswordHash(request.Password);

        user.DomainEvents.Add(result ? new UserAuthenticatedEvent(user) : new UserIncorrectPasswordEvent(user));
        
        if (result)
            user.DomainEvents.Add(new UserAuthorizedEvent(user));

        await dbContext.SaveChangesAsync(cancellationToken);

        return result ? user.Id : null;
    }
}