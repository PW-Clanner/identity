namespace Pw.Clanner.Identity.Common;

public abstract class DomainEvent
{
    public bool IsPublished { get; set; }

    public DateTime DateOccurred { get; protected set; }

    protected DomainEvent()
    {
        DateOccurred = DateTime.UtcNow;
    }
}