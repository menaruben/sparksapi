namespace SparksApi.Analyzer.Item;

public sealed record ItemAnalytic(
    string ChampionName,
    string ItemName,
    int ItemId,
    float WinRate,
    float PickRate,
    int TotalMatches
) : BaseAnalytic(WinRate, PickRate, TotalMatches);
