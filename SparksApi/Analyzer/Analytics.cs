namespace Sparks.Analyzer;

public abstract record Analytic(
    float WinRate,
    float PickRate,
    int TotalMatches
);