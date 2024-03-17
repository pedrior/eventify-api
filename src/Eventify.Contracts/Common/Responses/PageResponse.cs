namespace Eventify.Contracts.Common.Responses;

public record PageResponse<T>
{
    public int Page { get; init; }

    public int Limit { get; init; }

    public int Total { get; init; }

    public IEnumerable<T> Items { get; init; } = [];
}