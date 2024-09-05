using Microsoft.AspNetCore.Mvc;
using SparksApi.Api;
using SparksApi.Api.Handlers.Account;

namespace SparksApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AccountController(
    ILogger<AccountController> logger,
    AccountApiClient accountApiClient
    ) : ControllerBase
{
    private ILogger<AccountController> Logger => logger;
    private AccountApiClient AccountApi => accountApiClient;

    [HttpGet("accountFromRiotId", Name = "GetAccountFromRiotId")]
    public Account GetAccountFromRiotId(string name, string tagline, string region)
    {
        var actualRegion = ApiHelper.ParseRegion(region);
        Logger.LogInformation($"Getting account for {name}#{tagline} in {actualRegion}");
        var account = AccountApi.GetAccount(name, tagline, actualRegion).Result;
        Logger.LogInformation($"Account found: {account.GameName}#{account.TagLine}, {account.Puuid}");
        return account;
    }

    [HttpGet("GetAccountFromPuuid", Name = "GetAccountFromPuuid")]
    public Account GetAccountFromPuuid(string puuid, string region)
    {
        var actualRegion = ApiHelper.ParseRegion(region);
        Logger.LogInformation($"Getting account for {puuid} in {actualRegion}");
        var account = AccountApi.GetAccount(puuid, actualRegion).Result;
        Logger.LogInformation($"Account found: {account.GameName}#{account.TagLine}, {account.Puuid}");
        return account;
    }
}
