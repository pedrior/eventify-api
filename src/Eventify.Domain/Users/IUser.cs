using Eventify.Domain.Users.ValueObjects;

namespace Eventify.Domain.Users;

public interface IUser
{
    UserId Id { get; }
}