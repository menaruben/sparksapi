using SparksApi.Api.Handlers.Item;
using SparksApi.Api.Handlers.Match;
using SparksApi.Api.Handlers.Runes;

namespace SparksApi.Extensions;

public static class MatchExtension
{
    public static MatchParticipation ToParticipation(
        this Match match, string puuid, IItemApiClient itemApiClient, IRunesApiClient runesApiClient)
    {
        var participant = match.Participants.Single(p => p.Puuid == puuid);
        return new MatchParticipation(
            match.GameMode,
            $"https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/champion-icons/{participant.ChampionId}.png",
            participant.ChampionName,
            match.GetItems(puuid, itemApiClient).ToArray(),
            participant.Kills,
            participant.Deaths,
            participant.Assists,
            participant.Challenges.Kda,
            participant.Win,
            participant.TotalMinionsKilled + participant.NeutralMinionsKilled,
            runesApiClient.GetRunesFromPerks(participant.Perks)
        );
    }

    private static IEnumerable<Item> GetItems(this Match match, string puuid, IItemApiClient itemApiClient)
    {
        var participant = match.Participants.Single(p => p.Puuid == puuid);
        var itemIds = new[]
        {
            participant.Item0, participant.Item1, participant.Item2,
            participant.Item3, participant.Item4, participant.Item5
        };
        return itemIds
            .Where(i => i != ItemApiClient.EmptyItemSlotId)
            .Select(itemApiClient.GetItem)
            .DistinctBy(i => i.Id)
            .Where(i => i != null);
    }
}