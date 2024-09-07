﻿using SparksApi.Api.Handlers.Match;
using SparksApi.Extensions;

namespace SparksApi.Analyzer.Champion;

public sealed class ChampionAnalyzer : IAnalyzer<ChampionAnalytic>
{
    public AnalyticsCollection<ChampionAnalytic> Analyze(IEnumerable<MatchParticipation> participations)
    {
        var playedChampions = participations.GetPlayedChampions().ToArray();

        var championAnalytics = new List<ChampionAnalytic>();
        foreach (var champ in playedChampions)
        {
            var gamesPlayedWithChamp = participations.FilterByChampion(champ).ToArray();
            var gamesWonWithChamp = gamesPlayedWithChamp.Count(p => p.Win);
            var winrate = (float)gamesWonWithChamp / gamesPlayedWithChamp.Count();
            var pickrate = (float)gamesPlayedWithChamp.Count() / participations.Count();

            championAnalytics.Add(new(
                ChampionName: champ,
                WinRate: winrate,
                PickRate: pickrate,
                TotalMatches: gamesPlayedWithChamp.Count()
            ));
        }

        return AnalyticsCollection<ChampionAnalytic>.From(championAnalytics);
    }
}
