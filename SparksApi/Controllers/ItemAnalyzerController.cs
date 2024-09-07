using Microsoft.AspNetCore.Mvc;
using SparksApi.Analyzer;
using SparksApi.Analyzer.Item;
using SparksApi.Api;
using SparksApi.Api.Handlers.Match;

namespace SparksApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class ItemAnalyzerController(
    ILogger<ItemAnalyzerController> logger, ItemAnalyzer itemAnalyzer, MatchApiClient matchApiClient) : ControllerBase
{
    private ILogger<ItemAnalyzerController> Logger => logger;
    private IMatchApiClient MatchApiClient => matchApiClient;

    private ItemAnalyzer ItemAnalyzer => itemAnalyzer;

    [HttpGet(Name = "GetItemAnalytics")]
    public AnalyticsCollection<ItemAnalytic> Get(string puuid, string region, int matchCount = 10, int skip = 0)
    {
        Logger.LogInformation($"Getting item analytics for {puuid} in {region} with {matchCount} matches starting from {skip}");
        var actualRegion = ApiHelper.ParseRegion(region);
        var participations = MatchApiClient.GetMatchParticipations(puuid, actualRegion, matchCount, skip);
        Logger.LogInformation("Got participations successfully");
        var result = ItemAnalyzer.Analyze(participations.Result);
        Logger.LogInformation("Got analytics successfully");
        return result;
    }
}
