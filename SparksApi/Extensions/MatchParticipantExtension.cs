using SparksApi.Api.Handlers.Match;
using SparksApi.Api.Handlers.Runes;

namespace SparksApi.Extensions;

public static class MatchParticipantExtension
{
    public static IEnumerable<string> GetPlayedChampions(this IEnumerable<MatchParticipation> ps) =>
        ps
            .Select(p => p.Champion)
            .Distinct();

    public static IEnumerable<int> GetLegendaryItemsUsed(this IEnumerable<MatchParticipation> ps) =>
        ps
            .SelectMany(p => p.Items)
            .Select(i => i.Id)
            .Distinct();

    public static IEnumerable<Rune> GetRunesUsed(this IEnumerable<MatchParticipation> ps) =>
        ps
            .SelectMany(p => p.Runes)
            .Distinct();

    public static IEnumerable<MatchParticipation> FilterByChampion(this IEnumerable<MatchParticipation> ps, string championName) =>
        ps.Where(p => p.Champion == championName);


    public static IEnumerable<MatchParticipation> FilterByItem(this IEnumerable<MatchParticipation> ps, int itemId) =>
        ps.Where(p => p.Items.Any(i => i.Id == itemId));

    public static IEnumerable<MatchParticipation> FilterByRune(this IEnumerable<MatchParticipation> ps, int runeId) =>
        ps.Where(p => p.Runes.Any(r => r.Id == runeId));

    public static int CountMatchesPlayedWithTree(this IEnumerable<MatchParticipation> ps, string treeName) =>
        ps.Count(p => p.Runes.Any(r => r.TreeName == treeName));

    public static int CountMatchesWonWithTree(this IEnumerable<MatchParticipation> ps, string treeName) =>
        ps.Count(p => p.Runes.Any(r => r.TreeName == treeName && p.Win));
}