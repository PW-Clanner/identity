using MediatR;
using Pw.Clanner.Identity.Common;
using Pw.Clanner.Identity.Domain.Entities.Users;
using Pw.Clanner.Identity.Infrastructure.Persistence;

namespace Pw.Clanner.Identity.Features.Users;

internal sealed class RegisterUserCommandHandler(AppDbContext dbContext) : IRequestHandler<RegisterUserCommand, string>
{
    public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Email = request.Email,
            UserName = request.UserName
        };

        user.GeneratePasswordHash(request.Password);
        user.DomainEvents.Add(new UserRegisteredEvent(user));

        var entity = await dbContext.Users.AddAsync(user, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return entity.Entity.Id;
    }
}