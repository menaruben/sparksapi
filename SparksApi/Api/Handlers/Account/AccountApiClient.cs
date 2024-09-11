using System.Collections.Concurrent;
using System.Text.Json;

namespace SparksApi.Api.Handlers.Account;

public sealed class AccountApiClient(HttpClient httpClient, ILogger<AccountApiClient> logger) : IAccountApiClient
{
    private readonly ConcurrentDictionary<string, Account> _accountCache = new();
    private HttpClient Client => httpClient;
    private ILogger<AccountApiClient> Logger => logger;
    
    public async Task<string> GetPuuid(string gameName, string tagLine, Region region)
    {
        var puuid = GetPuuidFromCache(gameName, tagLine, region);
        if (puuid != null) return puuid;

        var url = $"{ApiHelper.GetRiotBaseUrlBasedOnRegion(region)}/riot/account/v1/accounts/by-riot-id/{gameName}/" +
                  $"{tagLine}?api_key={ApiHelper.GetApiKey()}";
        var response = await Client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        var account = JsonSerializer.Deserialize<Account>(json, ApiHelper.JsonOptions);
        if (account is null) throw new Exception("Account not found");
        Logger.LogInformation($"Successfully retrieved account: {account}");
        StoreAccountInCache(account with { Region = region });
        return account.Puuid;
    }

    public async Task<Account> GetAccount(string puuid, Region region)
    {
        var account = GetAccountFromCache(puuid);
        if (account is not null) return account;

        var url = $"{ApiHelper.GetRiotBaseUrlBasedOnRegion(region)}/riot/account/v1/accounts/by-puuid/{puuid}" +
                  $"?api_key={ApiHelper.GetApiKey()}";
        var response = await Client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        var accountNoRegion = JsonSerializer.Deserialize<Account>(json, ApiHelper.JsonOptions);
        if (accountNoRegion == null) throw new Exception("Account not found");
        account = accountNoRegion with { Region = region };
        StoreAccountInCache(account with { Region = region });
        return account;
    }

    public async Task<Account> GetAccount(string gameName, string tagLine, Region region)
    {
        var account = GetAccountFromCache(gameName, tagLine, region);
        Logger.LogInformation($"Retrieved account from cache: {account}");
        if (account != null) return account;

        var url = $"{ApiHelper.GetRiotBaseUrlBasedOnRegion(region)}/riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}" +
                  $"?api_key={ApiHelper.GetApiKey()}";
        var response = await Client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        var accountNoRegion = JsonSerializer.Deserialize<Account>(json, ApiHelper.JsonOptions);
        if (accountNoRegion is null) throw new Exception("Account not found");
        var accountWithRegion = accountNoRegion with { Region = region };
        StoreAccountInCache(accountWithRegion);
        return accountWithRegion;
    }

    private void StoreAccountInCache(Account account)
    {
        var key = $"{account.GameName}#{account.TagLine}#{account.Region}";
        _accountCache[key] = account;
        Logger.LogInformation($"Successfully stored account in cache: {account}");
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
