using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pw.Clanner.Identity.Common;
using Pw.Clanner.Identity.Domain.Entities;
using Pw.Clanner.Identity.Infrastructure.Persistence;

namespace Pw.Clanner.Identity.Features.Users;

/// <summary>
/// Запрос регистрации
/// </summary>
/// <param name="UserName">Имя пользователя</param>
/// <param name="Email">Электронная почта</param>
/// <param name="Password">Пароль</param>
/// <param name="ConfirmPassword">Подвтерждение пароля</param>
public record RegisterUserCommand(string UserName, string Email, string Password, string ConfirmPassword)
    : IRequest<string>;

public class RegisterUserController : ApiControllerBase
{
    [HttpPost("/api/users/register")]
    public async Task<ActionResult<string>> Register([FromBody] RegisterUserCommand command)
    {
        return await Mediatr.Send(command);
    }
}

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    private readonly AppDbContext _dbContext;

    public RegisterUserCommandValidator(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email обязательное поле")
            .EmailAddress().WithMessage("Неверный формат")
            .MaximumLength(255).WithMessage("Email не может быть длинее 255 символов")
            .MustAsync(BeUniqueEmail).WithMessage("Такой Email уже зарегистрирован");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Имя пользователя обязательное поле")
            .MaximumLength(255).WithMessage("Имя пользователя не может быть длинее 255 символов")
            .MustAsync(BeUniqueUserName).WithMessage("Такое имя пользователя уже зарегистрировано");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обязательное поле")
            .MinimumLength(6).WithMessage("Пароль не может быть короче 6 символов")
            .MaximumLength(255).WithMessage("Пароль не может быть длинее 255 символов");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Пароль обязательное поле")
            .Equal(v => v.Password).WithMessage("Пароли не совпадают");
    }

    private async Task<bool> BeUniqueUserName(RegisterUserCommand command, string userName,
        CancellationToken cancellationToken)
    {
        return !await _dbContext.Users
            .Where(x => x.UserName == userName)
            .AnyAsync(cancellationToken);
    }

    private async Task<bool> BeUniqueEmail(RegisterUserCommand command, string email,
        CancellationToken cancellationToken)
    {
        return !await _dbContext.Users
            .Where(x => x.Email != email)
            .AnyAsync(cancellationToken);
    }
}

internal sealed class RegisterUserCommandHandler(AppDbContext dbContext) : IRequestHandler<RegisterUserCommand, string>
{
    public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new UserEntity
        {
            Email = request.Email,
            UserName = request.UserName
        };

        user.GeneratePasswordHash(request.Password);

        var entity = await dbContext.Users.AddAsync(user, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return entity.Entity.Id;
    }
}