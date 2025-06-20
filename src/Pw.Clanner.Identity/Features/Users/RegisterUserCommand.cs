using MediatR;

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