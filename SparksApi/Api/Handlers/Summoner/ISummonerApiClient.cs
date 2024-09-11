namespace SparksApi.Api.Handlers.Summoner;

public interface ISummonerApiClient
{
    Task<Summoner> GetSummoner(string puuid, Region region);
}