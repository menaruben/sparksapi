using SparksApi.Api.Handlers.Runes;

namespace SparksApi.Api.Handlers.Match;

public sealed record MatchParticipation(
    string GameMode,
    string ChampionIconUrl,
    string Champion,
    Item.Item[] Items,
    int Kills,
    int Deaths,
    int Assists,
    float KillDeathAssistRatio,
    bool Win,
    int TotalCs,
    IEnumerable<Rune> Runes
);