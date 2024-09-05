using Microsoft.AspNetCore.Mvc;
using SparksApi.Analyzer.Rune;
using SparksApi.Analyzer;
using SparksApi.Api;
using SparksApi.Api.Handlers.Item;
using SparksApi.Api.Handlers.Match;
using SparksApi.Api.Handlers.Runes;
using SparksApi.Extensions;

namespace SparksApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class RuneAnalyzerController(
    ILogger<RuneAnalyzerController> logger,
    RunesApiClient runesApiClient,
    MatchApiClient matchApiClient,
    ItemApiClient itemApiClient,
    RuneAnalyzer runeAnalyzer
    ) : ControllerBase
{
    private ILogger<RuneAnalyzerController> Logger => logger;
    private IRunesApiClient RunesApiClient => runesApiClient;
    private IMatchApiClient MatchApiClient => matchApiClient;
    private IItemApiClient ItemApiClient => itemApiClient;
    private RuneAnalyzer RuneAnalyzer => runeAnalyzer;
    

    [HttpGet(Name = "GetRuneAnalytics")]
    public AnalyticsCollection<RuneTreeAnalytic> Get(string puuid, string region, int matchCount = 10, int skip = 0) {
        Logger.LogInformation($"Getting rune analytics for {puuid} in {region} with {matchCount} matches starting from {skip}");
        var actualRegion = ApiHelper.ParseRegion(region);
        var matchIds = MatchApiClient.GetMatchIds(puuid, actualRegion, matchCount, skip).Result;
        Logger.LogInformation($"Got following match ids: {string.Join(", ", matchIds)}");
        var matches = MatchApiClient.GetMatchesFromIds(matchIds, actualRegion).Result;
        Logger.LogInformation("Got matches from ids successfully");
        var participations = matches.GetParticipations(puuid, ItemApiClient, RunesApiClient);
        Logger.LogInformation("Got participations successfully");
        var result = RuneAnalyzer.Analyze(participations);
        Logger.LogInformation("Analyzed runes successfully");
        return result;
    }
}