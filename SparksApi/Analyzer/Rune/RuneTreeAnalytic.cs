namespace SparksApi.Analyzer.Rune;

public sealed record RuneTreeAnalytic(
    string TreeName,
    string ChampionName,
    float WinRate,
    float PickRate,
    int TotalMatches,
    IEnumerable<RuneAnalytic> Runes
) : BaseAnalytic(WinRate, PickRate, TotalMatches);
