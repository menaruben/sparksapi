using Microsoft.AspNetCore.Mvc;
using SparksApi.Api;
using SparksApi.Api.Handlers.Match;

namespace SparksApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class MatchController(ILogger<MatchController> logger, MatchApiClient matchApiClient) : ControllerBase
{
    private ILogger<MatchController> Logger => logger;
    private IMatchApiClient MatchApiClient => matchApiClient;

    [HttpGet("matchIds", Name = "GetMatchIds")]
    public async Task<string[]> GetMatchIds(string puuid, string region, int matchCount = 10, int skip = 0)
    {
        var actualRegion = ApiHelper.ParseRegion(region);
        Logger.LogInformation($"Getting {matchCount} match ids for {puuid} in {actualRegion} starting from {skip}");
        var matchIds = await MatchApiClient.GetMatchIds(puuid, actualRegion, matchCount, skip);
        Logger.LogInformation($"Got following match ids: {string.Join(", ", matchIds)}");
        return matchIds;
    }

    [HttpGet("matchesFromPuuid", Name = "GetMatchesFromPuuid")]
    public async Task<Match[]> GetMatchesWithPuuid(string puuid, string region, int matchCount = 10, int skip = 0)
    {
        var actualRegion = ApiHelper.ParseRegion(region);
        Logger.LogInformation($"Getting {matchCount} matches for {puuid} in {actualRegion} starting from {skip}");
        var matchIds = await MatchApiClient.GetMatchIds(puuid, actualRegion, matchCount, skip);
        Logger.LogInformation($"Got following match ids: {string.Join(", ", matchIds)}");
        var matches = await MatchApiClient.GetMatchesFromIds(matchIds, actualRegion);
        Logger.LogInformation("Got matches from ids successfully");
        return matches;
    }

    [HttpPost("matchesFromIds", Name = "GetMatchesFromIds")]
    public async Task<Match[]> GetMatchesFromIds([FromBody] string[] matchIds, string region)
    {
        var actualRegion = ApiHelper.ParseRegion(region);
        Logger.LogInformation($"Getting matches from {string.Join(", ", matchIds)} in {actualRegion}");
        var matches = await MatchApiClient.GetMatchesFromIds(matchIds, actualRegion);
        Logger.LogInformation("Got matches from ids successfully");
        return matches;
    }


    [HttpGet("matchHistory", Name = "GetMatchHistory")]
    public async Task<MatchHistory> GetMatchHistory(string puuid, string region, int matchCount = 10, int skip = 0)
    {
        var actualRegion = ApiHelper.ParseRegion(region);
        Logger.LogInformation($"Getting match history ({matchCount} matches) for {puuid} in {actualRegion} starting from {skip}");
        var participations = await MatchApiClient.GetMatchParticipations(puuid, actualRegion, matchCount, skip);
        var matchHistory = MatchHistory.From(participations);
        Logger.LogInformation("Got match history successfully");
        return matchHistory;
    }
}
