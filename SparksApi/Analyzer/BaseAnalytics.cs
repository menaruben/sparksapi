namespace SparksApi.Analyzer;

public abstract record BaseAnalytic(
    float WinRate,
    float PickRate,
    int TotalMatches
);