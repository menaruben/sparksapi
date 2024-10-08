﻿using SparksApi.Api.Handlers.Item;
using SparksApi.Api.Handlers.Runes;
using SparksApi.Api.Models;
using SparksApi.Extensions;
using System.Text.Json;

namespace SparksApi.Api.Handlers.Match;

public sealed class MatchApiClient(
    ItemApiClient itemApiClient,
    RunesApiClient runesApiClient,
    MatchCache matchDatabase,
    HttpClient httpClient
    ) : IMatchApiClient
{
    private HttpClient Client => httpClient;
    private IItemApiClient ItemApiClient => itemApiClient;
    private IRunesApiClient RunesApiClient => runesApiClient;
    private MatchCache MatchDatabase => matchDatabase;

    public async Task<Match[]> GetMatchesFromIds(IEnumerable<String> matchIds, Region region) =>
        await Task.WhenAll(matchIds.Select(matchId => GetMatch(matchId, region)));

    public async Task<MatchParticipation[]> GetMatchParticipations(
        string puuid, Region region, int count, int skip)
    {
        var matches = await GetMatchIds(puuid, region, count, skip);
        var matchCollection = await GetMatchesFromIds(matches, region);
        return matchCollection.GetParticipations(puuid, ItemApiClient, RunesApiClient).ToArray();
    }

    public async Task<string[]> GetMatchIds(string puuid, Region region, int count, int skip)
    {
        var url =
            $"{ApiHelper.GetRiotBaseUrlBasedOnRegion(region)}" +
            $"/lol/match/v5/matches/by-puuid/{puuid}" +
            $"/ids?start={skip}&count={count}&api_key={ApiHelper.GetApiKey()}";

        var response = await Client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<string[]>(json, ApiHelper.JsonOptions)
               ?? throw new Exception("Failed to deserialize match ids");
    }

    private async Task<Match> GetMatch(string matchId, Region region)
    {
        // if (_matchCache.TryGetValue(matchId, out var maybeMatch)) return maybeMatch;
        var maybeMatch = MatchDatabase.TryGet(matchId);
        if (maybeMatch is not null) return maybeMatch;

        var url =
            ApiHelper.GetRiotBaseUrlBasedOnRegion(region) +
            $"/lol/match/v5/matches/{matchId}" +
            $"?api_key={ApiHelper.GetApiKey()}";

        var response = await Client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        var matchDto = JsonSerializer.Deserialize<MatchDto>(json, ApiHelper.JsonOptions);
        if (matchDto is null) throw new Exception("Failed to deserialize match");
        var match = new Match(
            matchDto.Metadata.MatchId,
            matchDto.Metadata.Participants,
            matchDto.Info.Participants,
            GameModeToDisplayName(matchDto.Info.GameMode)
        );
        MatchDatabase.TryAdd(matchId, match);
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
