using SparksApi.Api.Handlers.Item;
using SparksApi.Api.Handlers.Match;
using SparksApi.Extensions;

namespace SparksApi.Analyzer.Item;

public sealed class ItemAnalyzer(ItemApiClient itemApiClient) : IAnalyzer<ItemAnalytic>
{
    private ItemApiClient ItemApi => itemApiClient;

    public AnalyticsCollection<ItemAnalytic> Analyze(IEnumerable<MatchParticipation> participations)
    {
        var playedChampions = participations.GetPlayedChampions();

        var itemAnalytics = new List<ItemAnalytic>();
        foreach (var champion in playedChampions)
        {
            var itemAnalyticsForChamp = AnalyzeItemsForChampion(participations, champion);
            itemAnalytics.AddRange(itemAnalyticsForChamp);
        }

        return AnalyticsCollection<ItemAnalytic>.From(itemAnalytics);
    }

    private IEnumerable<ItemAnalytic> AnalyzeItemsForChampion(IEnumerable<MatchParticipation> ps, string championName)
    {
        var gamesPlayedWithChamp = ps.FilterByChampion(championName).ToArray();
        var items = gamesPlayedWithChamp.GetLegendaryItemsUsed();

        var analytics = new List<ItemAnalytic>();
        foreach (var itemId in items)
        {
            if (analytics.Any(a => a.ItemId == itemId)) continue;

            var gamesWithItemAndChamp = gamesPlayedWithChamp.FilterByItem(itemId).ToArray();
            var gamesWonWithChampAndItem = gamesWithItemAndChamp.Count(p => p.Win);
            var winRate = (float)gamesWonWithChampAndItem / gamesPlayedWithChamp.Length;
            var pickRate = (float)gamesWithItemAndChamp.Length / gamesPlayedWithChamp.Length;
            var itemName = ItemApi.GetItem(itemId).Name;
            analytics.Add(new ItemAnalytic(championName, itemName, itemId, winRate, pickRate, gamesWithItemAndChamp.Length));
        }

        return analytics;
    }
}
