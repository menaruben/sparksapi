namespace SparksApi.Api.Handlers.Runes;

public sealed record Rune(
    int Id,
    string Name,
    string Description,
    string TreeName,
    string IconUrl
);