namespace SparksApi.Api.Models;

public sealed record ItemDto(
    int Id,
    string Name,
    string Description,
    IEnumerable<string> Categories,
    int PriceTotal
);