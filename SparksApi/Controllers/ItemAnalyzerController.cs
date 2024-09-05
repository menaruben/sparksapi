using Microsoft.AspNetCore.Mvc;
using SparksApi.Analyzer;
using SparksApi.Analyzer.Item;
using SparksApi.Api;
using SparksApi.Api.Handlers.Item;
using SparksApi.Api.Handlers.Match;
using SparksApi.Api.Handlers.Runes;
using SparksApi.Extensions;

namespace SparksApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class ItemAnalyzerController(
    ILogger<ItemAnalyzerController> logger,
    ItemAnalyzer itemAnalyzer,
    MatchApiClient matchApiClient,
    ItemApiClient itemApiClient,
    RunesApiClient runesApiClient
    ) : ControllerBase
{
    private ILogger<ItemAnalyzerController> Logger => logger;
    private IMatchApiClient MatchApiClient => matchApiClient;
    private IItemApiClient ItemApiClient => itemApiClient;
    private IRunesApiClient RunesApiClient => runesApiClient;
    
    private ItemAnalyzer ItemAnalyzer => itemAnalyzer;

    [HttpGet(Name = "GetItemAnalytics")]
    public AnalyticsCollection<ItemAnalytic> Get(string puuid, string region, int matchCount = 10, int skip = 0)
    {
        Logger.LogInformation($"Getting item analytics for {puuid} in {region} with {matchCount} matches starting from {skip}");
        var actualRegion = ApiHelper.ParseRegion(region);
        var matchIds = MatchApiClient.GetMatchIds(puuid, actualRegion, matchCount, skip).Result;
        Logger.LogInformation($"Got following match ids: {string.Join(", ", matchIds)}");
        var matches = MatchApiClient.GetMatchesFromIds(matchIds, actualRegion).Result;
        Logger.LogInformation("Got matches from ids successfully");
        var participations = matches.GetParticipations(puuid, ItemApiClient, RunesApiClient);
        Logger.LogInformation("Got participations successfully");
        var result = ItemAnalyzer.Analyze(participations);
        Logger.LogInformation("Got analytics successfully");
        return result;
    }
}
