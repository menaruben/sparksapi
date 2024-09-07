using Microsoft.AspNetCore.Mvc;
using SparksApi.Api;
using SparksApi.Api.Handlers.Summoner;

namespace SparksApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class SummonerController(
    ILogger<SummonerController> logger,
    SummonerApiClient summonerApiClient
) : ControllerBase
{
    private ILogger<SummonerController> Logger => logger;
    private SummonerApiClient SummonerApiClient => summonerApiClient;

    [HttpGet("summoner", Name = "GetSummoner")]
    public Summoner GetSummoner(string puuid, string region)
    {
        var actualRegion = ApiHelper.ParseRegion(region);
        Logger.LogInformation($"Getting account for {puuid} in {actualRegion}");
        var summoner = SummonerApiClient.GetSummoner(puuid, actualRegion);
        Logger.LogInformation($"Account found: {summoner.Puuid}, {summoner.ProfileIconUrl}");
        return summoner;
    }
}