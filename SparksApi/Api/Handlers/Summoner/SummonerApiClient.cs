using SparksApi.Api.Models;
using System.Text.Json;

namespace SparksApi.Api.Handlers.Summoner;

public sealed class SummonerApiClient(HttpClient httpClient) : ISummonerApiClient {
    private HttpClient Client => httpClient;
    private readonly string _profileIconBaseUrl = "/game/assets/ux/summonericons/profileicon";

    public async Task<Summoner> GetSummoner(string puuid, Region region)
    {
        var url = $"{ApiHelper.GetRiotSummonerBaseUrl(region)}/lol/summoner/v4/summoners/by-puuid/{puuid}" +
                  $"?api_key={ApiHelper.GetApiKey()}";
        var response = await Client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        var summonerDto = JsonSerializer.Deserialize<SummonerDto>(json, ApiHelper.JsonOptions);
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