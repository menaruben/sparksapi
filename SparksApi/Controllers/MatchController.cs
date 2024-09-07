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
    public IEnumerable<string> GetMatchIds(string puuid, string region, int matchCount = 10, int skip = 0)
    {
        var actualRegion = ApiHelper.ParseRegion(region);
        Logger.LogInformation($"Getting {matchCount} match ids for {puuid} in {actualRegion} starting from {skip}");
        var matchIds = MatchApiClient.GetMatchIds(puuid, actualRegion, matchCount, skip).Result.ToArray();
        Logger.LogInformation($"Got following match ids: {string.Join(", ", matchIds)}");
        return matchIds;
    }

    [HttpGet("matchesFromPuuid", Name = "GetMatchesFromPuuid")]
    public MatchCollection GetMatchesWithPuuid(string puuid, string region, int matchCount = 10, int skip = 0)
    {
        var actualRegion = ApiHelper.ParseRegion(region);
        Logger.LogInformation($"Getting {matchCount} matches for {puuid} in {actualRegion} starting from {skip}");
        var matchIds = MatchApiClient.GetMatchIds(puuid, actualRegion, matchCount, skip).Result.ToArray();
        Logger.LogInformation($"Got following match ids: {string.Join(", ", matchIds)}");
        var matches = MatchApiClient.GetMatchesFromIds(matchIds, actualRegion).Result;
        Logger.LogInformation("Got matches from ids successfully");
        return matches;
    }

    [HttpPost("matchesFromIds", Name = "GetMatchesFromIds")]
    public MatchCollection GetMatchesFromIds([FromBody] IEnumerable<string> matchIds, string region)
    {
        var actualRegion = ApiHelper.ParseRegion(region);
        Logger.LogInformation($"Getting matches from {string.Join(", ", matchIds)} in {actualRegion}");
        var matches = MatchApiClient.GetMatchesFromIds(matchIds, actualRegion).Result;
        Logger.LogInformation("Got matches from ids successfully");
        return matches;
    }


    [HttpGet("matchHistory", Name = "GetMatchHistory")]
    public MatchHistory GetMatchHistory(string puuid, string region, int matchCount = 10, int skip = 0)
    {
        var actualRegion = ApiHelper.ParseRegion(region);
        Logger.LogInformation($"Getting match history ({matchCount} matches) for {puuid} in {actualRegion} starting from {skip}");
        var matchHistory = MatchHistory.From(MatchApiClient.GetMatchParticipations(puuid, actualRegion, matchCount, skip).Result);
        Logger.LogInformation("Got match history successfully");
        return matchHistory;
    }
}
