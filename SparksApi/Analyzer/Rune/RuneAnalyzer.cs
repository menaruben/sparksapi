using SparksApi.Api.Handlers.Match;
using SparksApi.Extensions;

namespace SparksApi.Analyzer.Rune;

public sealed class RuneAnalyzer : IAnalyzer<RuneTreeAnalytic>
{
    public AnalyticsCollection<RuneTreeAnalytic> Analyze(IEnumerable<MatchParticipation> participation)
    {
        var playedChampions = participation.GetPlayedChampions().ToArray();
        var runeAnalytics = new List<RuneTreeAnalytic>();

        foreach (var champion in playedChampions)
        {
            var runeAnalyticForChamp = AnalyzeRunesForChampion(participation
                .FilterByChampion(champion), champion).ToArray();

            var runeTreeAnalytics = runeAnalyticForChamp
                .GroupBy(r => r.Rune.TreeName)
                .SelectMany(tree => RuneTreeAnalyticsForChamp(
                    tree, runeAnalyticForChamp, champion, participation));

            runeAnalytics.AddRange(runeTreeAnalytics);
        }

        return AnalyticsCollection<RuneTreeAnalytic>.From(runeAnalytics);
    }

    private IEnumerable<RuneTreeAnalytic> RuneTreeAnalyticsForChamp(IGrouping<string, RuneAnalytic> tree,
        IEnumerable<RuneAnalytic> runeAnalyticsForChamp, string championName, IEnumerable<MatchParticipation> ps)
    {
        var treeNames = tree.Select(r => r.Rune.TreeName).Distinct().ToArray();
        return treeNames.Select(treeName =>
        {
            var runes = runeAnalyticsForChamp.Where(r => r.Rune.TreeName == treeName).ToArray();
            var winRate = (float)ps.CountMatchesWonWithTree(treeName) / ps.CountMatchesPlayedWithTree(treeName);
            var pickRate = (float)ps.CountMatchesPlayedWithTree(treeName) / ps.Count();
            var totalMatchesWithTree = runes.MaxBy(r => r.TotalMatches)!.TotalMatches;
            return new RuneTreeAnalytic(treeName, championName, winRate, pickRate, totalMatchesWithTree, runes);
        });
    }


    private IEnumerable<RuneAnalytic> AnalyzeRunesForChampion(IEnumerable<MatchParticipation> ps, string championName)
    {
        var gamesPlayedWithChamp = ps.FilterByChampion(championName).ToArray();
        var runes = gamesPlayedWithChamp.GetRunesUsed().ToArray();

        var analytics = new List<RuneAnalytic>();
        foreach (var rune in runes)
        {
            if (analytics.Any(a => a.Rune == rune)) continue;

            var gamesWithRuneAndChamp = gamesPlayedWithChamp.Where(p => p.Runes.Contains(rune)).ToArray();
            var gamesWonWithChampAndRune = gamesWithRuneAndChamp.Count(p => p.Win);
            var winRate = (float)gamesWonWithChampAndRune / gamesWithRuneAndChamp.Length;
            var pickRate = (float)gamesWithRuneAndChamp.Length / gamesPlayedWithChamp.Length;
            analytics.Add(new RuneAnalytic(rune, winRate, pickRate, gamesWithRuneAndChamp.Length));
        }

        return analytics;
    }
}