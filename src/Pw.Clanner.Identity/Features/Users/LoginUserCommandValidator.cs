using FluentValidation;

namespace Pw.Clanner.Identity.Features.Users;

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