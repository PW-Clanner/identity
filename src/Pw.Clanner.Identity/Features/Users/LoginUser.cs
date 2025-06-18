using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pw.Clanner.Identity.Common;
using Pw.Clanner.Identity.Infrastructure.Persistence;

namespace Pw.Clanner.Identity.Features.Users;

public record LoginUserCommand(string UserName, string Password, bool RememberMe)
    : IRequest<bool>;

public class LoginUserController : ApiControllerBase
{
    [HttpPost("/api/users/login")]
    public async Task<ActionResult<bool>> Login([FromBody] LoginUserCommand command)
    {
        return await Mediatr.Send(command);
    }
}

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Имя пользователя обязательное поле")
            .MaximumLength(255).WithMessage("Имя пользователя не может быть длинее 255 символов");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обязательное поле")
            .MinimumLength(6).WithMessage("Пароль не может быть короче 6 символов")
            .MaximumLength(255).WithMessage("Пароль не может быть длинее 255 символов");
    }
}

internal sealed class LoginUserCommandHandler(AppDbContext dbContext) : IRequestHandler<LoginUserCommand, bool>
{
    public async Task<bool> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var userName = request.UserName.ToLower();
        var user = await dbContext.Users
            .Where(x => x.UserName.ToLower() == userName)
            .SingleOrDefaultAsync(cancellationToken);

        if (user is null)
            return false;

        var result = user.VerifyPasswordHash(request.Password);

        return result;
    }
}