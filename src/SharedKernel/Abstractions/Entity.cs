using System.ComponentModel.DataAnnotations.Schema;

namespace SharedKernel.Abstractions;

public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents;

    protected Entity()
    {
        Id = Guid.NewGuid();
        DateCreatedUtc = DateTime.UtcNow;
        _domainEvents = [];
    }

    public Guid Id { get; }

    public DateTime DateCreatedUtc { get; }

    [NotMapped]
    public IReadOnlyCollection<IDomainEvent> DomainEvents => [.. _domainEvents];

    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);

    public IDomainEvent[] ClearDomainEvents()
    {
        IDomainEvent[] dequeuedEvents = [.. _domainEvents];

        _domainEvents.Clear();

        return dequeuedEvents;
    }
}