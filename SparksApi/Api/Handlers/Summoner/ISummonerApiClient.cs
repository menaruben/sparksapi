namespace SparksApi.Api.Handlers.Summoner;

public interface ISummonerApiClient
{
    Summoner GetSummoner(string puuid, Region region);
}