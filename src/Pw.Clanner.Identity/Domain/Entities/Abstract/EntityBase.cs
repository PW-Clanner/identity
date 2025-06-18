namespace Pw.Clanner.Identity.Domain.Entities.Abstract;

public class EntityBase : IEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
}