using Sparks.Analyzer;

namespace SparksApi.Analyzer.Rune;

public sealed record RuneAnalytic(
    SparksApi.Api.Handlers.Runes.Rune Rune,
    float WinRate,
    float PickRate,
    int TotalMatches
) : Analytic(WinRate, PickRate, TotalMatches);