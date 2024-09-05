using SparksApi.Api.Handlers.Item;
using SparksApi.Api.Handlers.Runes;
using SparksApi.Api.Models;
using System.Collections.Concurrent;
using SparksApi.Extensions;

namespace SparksApi.Api.Handlers.Match;

public sealed class MatchApiClient : IMatchApiClient
{
    private readonly HttpClient _client = new ();
    private readonly ApiHelper _apiHelper = new ();
    private readonly IItemApiClient _itemApiClient = new ItemApiClient();
    private readonly IRunesApiClient _runesApiClient = new RunesApiClient();
    private readonly ConcurrentDictionary<string, Match> _matchCache = new();
    
    public async Task<MatchCollection> GetMatchesFromIds(IEnumerable<String> matchIds, Region region) =>
        MatchCollection.From(await Task.WhenAll(matchIds.Select(matchId => GetMatch(matchId, region))));

    public async Task<IEnumerable<MatchParticipation>> GetMatchParticipations(
        string puuid, Region region, int count, int skip)
    {
        var matches = await GetMatchIds(puuid, region, count, skip);
        var matchCollection = await GetMatchesFromIds(matches, region);
        return matchCollection.GetParticipations(puuid, _itemApiClient, _runesApiClient);
    }

    public async Task<IEnumerable<String>> GetMatchIds(string puuid, Region region, int count, int skip)
    {
        var url =
            $"{_apiHelper.GetRiotBaseUrlBasedOnRegion(region)}" +
            $"/lol/match/v5/matches/by-puuid/{puuid}" +
            $"/ids?start={skip}&count={count}&api_key={_apiHelper.GetApiKey()}";

        var response = await _client.GetAsync(url);
        return await _apiHelper.MapResponse<IEnumerable<String>>(response);
    }

    private async Task<Match> GetMatch(string matchId, Region region)
    {
        if (_matchCache.TryGetValue(matchId, out var maybeMatch)) return maybeMatch;

        var url =
            _apiHelper.GetRiotBaseUrlBasedOnRegion(region) +
            $"/lol/match/v5/matches/{matchId}" +
            $"?api_key={_apiHelper.GetApiKey()}";

        var response = await _client.GetAsync(url);
        var matchDto = await _apiHelper.MapResponse<MatchDto>(response);
        var match = new Match(
            matchDto.Metadata.MatchId,
            matchDto.Metadata.Participants,
            matchDto.Info.Participants,
            GameModeToDisplayName(matchDto.Info.GameMode)
        );
        _matchCache[matchId] = match;
        return match;
    }
    
    private static string GameModeToDisplayName(string gameMode) =>
        gameMode switch
        {
            "CLASSIC" => "Summoner's Rift",
            "ARAM" => "ARAM",
            "URF" => "URF",
            "ONEFORALL" => "One for All",
            "NEXUSBLITZ" => "Nexus Blitz",
            "CHERRY" => "Arena",
            _ => gameMode
        };
}
