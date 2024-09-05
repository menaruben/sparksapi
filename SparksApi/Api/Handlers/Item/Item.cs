namespace SparksApi.Api.Handlers.Item;

public sealed record Item(
    int Id,
    string Name,
    string Description,
    IEnumerable<string> Stats,
    string IconUrl,
    int PriceTotal
);
