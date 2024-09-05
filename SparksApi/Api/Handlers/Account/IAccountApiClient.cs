using SparksApi.Api;
using SparksApi.Api.Handlers.Account;

public interface IAccountApiClient
{
    Task<String> GetPuuid(string gameName, string tagLine, Region region);
    Task<Account> GetAccount(string puuid, Region region);
    Task<Account> GetAccount(string gameName, string tagLine, Region region);
}
