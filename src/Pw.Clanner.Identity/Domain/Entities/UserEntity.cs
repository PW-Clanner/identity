using Pw.Clanner.Identity.Domain.Entities.Abstract;

namespace Pw.Clanner.Identity.Domain.Entities;

public class UserEntity : EntityBase
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Хэш пароля
    /// </summary>
    public string PasswordHash { get; set; }

    /// <summary>
    /// Почтовый адрес
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Дата и время последнего входа
    /// </summary>
    public DateTime LastLogin { get; set; }
}