using Microsoft.AspNetCore.Mvc;
using SparksApi.Analyzer;
using SparksApi.Analyzer.Champion;
using SparksApi.Api;
using SparksApi.Api.Handlers.Item;
using SparksApi.Api.Handlers.Match;
using SparksApi.Api.Handlers.Runes;

namespace SparksApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class ChampionAnalyzerController(
    ILogger<ChampionAnalyzerController> logger,
    MatchApiClient matchApiClient,
    ItemApiClient itemApiClient,
    RunesApiClient runesApiClient,
    ChampionAnalyzer championAnalyzer
    ) : ControllerBase
{
    private ILogger<ChampionAnalyzerController> Logger => logger;
    private IMatchApiClient MatchApiClient => matchApiClient;
    private IItemApiClient ItemApiClient => itemApiClient;
    private IRunesApiClient RunesApiClient => runesApiClient;
    private ChampionAnalyzer ChampionAnalyzer => championAnalyzer;


    [HttpGet(Name = "GetChampionAnalytics")]
    public AnalyticsCollection<ChampionAnalytic> Get(string puuid, string region, int matchCount = 10, int skip = 0)
    {
        Logger.LogInformation($"Getting champion analytics for {puuid} in {region} with {matchCount} matches starting from {skip}");
        var actualRegion = ApiHelper.ParseRegion(region);
        Logger.LogInformation($"Getting {matchCount} matches for {puuid} in {actualRegion} starting from {skip}");
        var participations = MatchApiClient.GetMatchParticipations(puuid, actualRegion, matchCount, skip);
        Logger.LogInformation("Got participations successfully from matches");
        var result = ChampionAnalyzer.Analyze(participations.Result);
        Logger.LogInformation("Got champion analytics successfully");
        return result;
    }
}
