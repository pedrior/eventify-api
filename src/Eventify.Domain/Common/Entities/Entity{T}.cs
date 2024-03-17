namespace Eventify.Domain.Common.Entities;

public abstract class Entity<TId>(TId id) : Entity, IEntity<TId>, IEquatable<Entity<TId>> where TId : notnull
{
    public virtual TId Id { get; } = id;

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right) => Equals(left, right);

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right) => !Equals(left, right);

    public bool Equals(Entity<TId>? other)
    {
        return !ReferenceEquals(null, other) 
               && (ReferenceEquals(this, other) || EqualityComparer<TId>.Default.Equals(Id, other.Id));
    }

    public override bool Equals(object? obj)
    {
        return !ReferenceEquals(null, obj) &&
               (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((Entity<TId>)obj));
    }

    public override int GetHashCode() => EqualityComparer<TId>.Default.GetHashCode(Id);
}