using Microsoft.AspNetCore.Mvc;
using SparksApi.Analyzer;
using SparksApi.Analyzer.Rune;
using SparksApi.Api;
using SparksApi.Api.Handlers.Match;

namespace SparksApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class RuneAnalyzerController(
    ILogger<RuneAnalyzerController> logger, MatchApiClient matchApiClient, RuneAnalyzer runeAnalyzer) : ControllerBase
{
    private ILogger<RuneAnalyzerController> Logger => logger;
    private IMatchApiClient MatchApiClient => matchApiClient;
    private RuneAnalyzer RuneAnalyzer => runeAnalyzer;


    [HttpGet(Name = "GetRuneAnalytics")]
    public async Task<RuneTreeAnalytic[]> Get(string puuid, string region, int matchCount = 10, int skip = 0)
    {
        Logger.LogInformation($"Getting rune analytics for {puuid} in {region} with {matchCount} matches starting from {skip}");
        var actualRegion = ApiHelper.ParseRegion(region);
        var participations = await MatchApiClient.GetMatchParticipations(puuid, actualRegion, matchCount, skip);
        Logger.LogInformation("Got participations successfully");
        var result = RuneAnalyzer.Analyze(participations);
        Logger.LogInformation("Analyzed runes successfully");
        return result;
    }
}