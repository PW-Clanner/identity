using Pw.Clanner.Identity.Common;
using Pw.Clanner.Identity.Common.Interfaces;
using Pw.Clanner.Identity.Domain.Entities.Abstract;

namespace Pw.Clanner.Identity.Domain.Entities.Users;

public class User : EntityBase, IHasDomainEvent
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
    
    /// <summary>
    /// Аудит
    /// </summary>
    public List<UserAudit> Audits { get; set; }

    public List<DomainEvent> DomainEvents { get; } = [];
}