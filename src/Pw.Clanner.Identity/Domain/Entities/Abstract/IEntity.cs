namespace Pw.Clanner.Identity.Domain.Entities.Abstract;

public interface IEntity
{
    string Id { get; set; }
    
    DateTime CreatedAt { get; set; }
    
    DateTime? ModifiedAt { get; set; }
}