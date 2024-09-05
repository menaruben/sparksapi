using Microsoft.AspNetCore.Mvc;
using SparksApi.Api;
using SparksApi.Api.Handlers.Account;
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
    private SummonerApiClient _summonerApiClient => summonerApiClient;

    [HttpGet("summoner", Name = "GetSummoner")]
    public Summoner GetSummoner(string puuid, string region)
    {
        var actualRegion = ApiHelper.ParseRegion(region);
        Logger.LogInformation($"Getting account for {puuid} in {actualRegion}");
        var summoner = _summonerApiClient.GetSummoner(puuid, actualRegion);
        Logger.LogInformation($"Account found: {summoner.Puuid}, {summoner.ProfileIconUrl}");
        return summoner;
    }
}