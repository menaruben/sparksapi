using Sparks.Analyzer;

namespace SparksApi.Analyzer.Champion;

public sealed record ChampionAnalytic(
    string ChampionName,
    float WinRate,
    float PickRate,
    int TotalMatches
) : Analytic(WinRate, PickRate, TotalMatches);
