using SparksApi.Api.Handlers.Item;
using SparksApi.Api.Handlers.Match;
using SparksApi.Api.Handlers.Runes;

namespace SparksApi.Extensions;

public static class MatchCollectionExtension
{
    public static IEnumerable<MatchParticipation> GetParticipations(
        this MatchCollection matches, string puuid, IItemApiClient itemApiClient, IRunesApiClient runesApiClient) =>
        matches.Select(m => m.ToParticipation(puuid, itemApiClient, runesApiClient));
}
