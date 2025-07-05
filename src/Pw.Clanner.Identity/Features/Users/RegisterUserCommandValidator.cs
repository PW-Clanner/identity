using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Pw.Clanner.Identity.Infrastructure.Persistence;

namespace Pw.Clanner.Identity.Features.Users;

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
            .Where(x => x.Email == email)
            .AnyAsync(cancellationToken);
    }
}