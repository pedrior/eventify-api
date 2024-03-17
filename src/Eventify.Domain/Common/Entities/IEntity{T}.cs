namespace Eventify.Domain.Common.Entities;

public interface IEntity<out TId> : IEntity where TId : notnull
{
    TId Id { get; }
}