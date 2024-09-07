using SparksApi.Api.Models;
using System.Text.Json;

namespace SparksApi.Api.Handlers.Summoner;

public sealed class SummonerApiClient : ISummonerApiClient
{
    private readonly HttpClient _client = new();
    private readonly string _profileIconBaseUrl;

    public SummonerApiClient() =>
        _profileIconBaseUrl = ApiHelper.CdragonBaseUrl + "/game/assets/ux/summonericons/profileicon";

    public Summoner GetSummoner(string puuid, Region region)
    {
        var url = $"{ApiHelper.GetRiotSummonerBaseUrl(region)}/lol/summoner/v4/summoners/by-puuid/{puuid}" +
                  $"?api_key={ApiHelper.GetApiKey()}";
        var response = _client.GetAsync(url);
        var json = response.Result.Content.ReadAsStringAsync();
        var summonerDto = JsonSerializer.Deserialize<SummonerDto>(json.Result, ApiHelper.JsonOptions);
        if (summonerDto is null) throw new Exception("Couldn't deserialize summoner");
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