using SparksApi.Api.Models;

namespace SparksApi.Api.Handlers.Summoner;

public sealed class SummonerApiClient : ISummonerApiClient {
    private readonly HttpClient _client = new ();
    private readonly ApiHelper _apiHelper = new ();
    private readonly string _profileIconBaseUrl;
    
    public SummonerApiClient() =>
        _profileIconBaseUrl = ApiHelper.CdragonBaseUrl + "/game/assets/ux/summonericons/profileicon";
    
    public Summoner GetSummoner(string puuid, Region region) {
        var url = $"{_apiHelper.GetRiotSummonerBaseUrl(region)}/lol/summoner/v4/summoners/by-puuid/{puuid}?api_key={_apiHelper.GetApiKey()}";
        var response = _client.GetAsync(url);
        var summonerDto = _apiHelper.MapResponse<SummonerDto>(response.Result).Result;
        return new(
            summonerDto.AccountId,
            GetProfileIconUrl(summonerDto.ProfileIconId),
            summonerDto.RevisionDate,
            summonerDto.Id,
            summonerDto.Puuid,
            summonerDto.SummonerLevel
        );
    }
    
    private string GetProfileIconUrl(int profileIconId) =>
        $"{_profileIconBaseUrl}{profileIconId}.png";
}