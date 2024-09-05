using System.Collections.Concurrent;

namespace SparksApi.Api.Handlers.Account;

public sealed class AccountApiClient : IAccountApiClient
{
    private readonly HttpClient _client = new();
    private readonly ApiHelper _apiHelper = new();
    private readonly ConcurrentDictionary<string, Account> _accountCache = new();

    public async Task<String> GetPuuid(string gameName, string tagLine, Region region)
    {
        var puuid = GetPuuidFromCache(gameName, tagLine, region);
        if (puuid != null) return puuid;

        var url = $"{_apiHelper.GetRiotBaseUrlBasedOnRegion(region)}/riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}?api_key={_apiHelper.GetApiKey()}";
        var response = await _client.GetAsync(url);
        var account = await _apiHelper.MapResponse<Account>(response);
        StoreAccountInCache(account with { Region = region });
        return account.Puuid;
    }

    public async Task<Account> GetAccount(string puuid, Region region)
    {
        var account = GetAccountFromCache(puuid);
        if (account != null) return account;

        var url = $"{_apiHelper.GetRiotBaseUrlBasedOnRegion(region)}/riot/account/v1/accounts/by-puuid/{puuid}?api_key={_apiHelper.GetApiKey()}";
        var response = await _client.GetAsync(url);
        var accountNoRegion = await _apiHelper.MapResponse<Account>(response);
        account = accountNoRegion with { Region = region };
        StoreAccountInCache(account with { Region = region });
        return account;
    }

    public async Task<Account> GetAccount(string gameName, string tagLine, Region region)
    {
        var account = GetAccountFromCache(gameName, tagLine, region);
        if (account != null) return account;

        var url = $"{_apiHelper.GetRiotBaseUrlBasedOnRegion(region)}/riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}?api_key={_apiHelper.GetApiKey()}";
        var response = await _client.GetAsync(url);
        var accountNoRegion = await _apiHelper.MapResponse<Account>(response);
        StoreAccountInCache(accountNoRegion with { Region = region });
        return accountNoRegion with { Region = region };
    }

    private void StoreAccountInCache(Account account)
    {
        var key = $"{account.GameName}#{account.TagLine}#{account.Region}";
        _accountCache[key] = account;
    }

    private Account? GetAccountFromCache(string gameName, string tagLine, Region region)
    {
        var puuid = GetPuuidFromCache(gameName, tagLine, region);
        return puuid != null
            ? GetAccountFromCache(puuid)
            : null;
    }

    private Account? GetAccountFromCache(string puuid) =>
        _accountCache.Values.FirstOrDefault(account => account.Puuid == puuid);

    private String? GetPuuidFromCache(string gameName, string tagLine, Region region) =>
        _accountCache.TryGetValue($"{gameName}#{tagLine}#{region}", out var account)
            ? account.Puuid
            : null;
}
