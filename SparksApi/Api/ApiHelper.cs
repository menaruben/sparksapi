using System.Text.Json;

namespace SparksApi.Api;

public sealed class ApiHelper
{
    public static readonly string DdragonBaseUrl = "https://ddragon.leagueoflegends.com/cdn";
    public static readonly string CdragonBaseUrl = "https://raw.communitydragon.org/latest";

    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        IgnoreReadOnlyFields = true
    };

    // public static async Task<T> MapResponse<T>(HttpResponseMessage response)
    // {
    //     if (response.IsSuccessStatusCode)
    //     {
    //         var json = await response.Content.ReadAsStringAsync();
    //         var result = JsonSerializer.Deserialize<T>(json, Options);
    //
    //         if (result is null)
    //         {
    //             throw new Exception("Failed to deserialize response");
    //         }
    //
    //         return result;
    //     }
    //
    //     throw new Exception($"Failed to get response: {response.StatusCode}");
    // }

    public static string GetApiKey() =>
        Environment.GetEnvironmentVariable("RIOT_API_KEY") ?? throw new Exception("API_KEY not found");

    public static string GetRiotBaseUrlBasedOnRegion(Region region) => region switch
    {
        Region.Na => "https://americas.api.riotgames.com",
        Region.Euw => "https://europe.api.riotgames.com",
        Region.Eun => "https://europe.api.riotgames.com",
        Region.Kr => "https://asia.api.riotgames.com",
        Region.Br => "https://americas.api.riotgames.com",
        Region.Jp => "https://asia.api.riotgames.com",
        Region.Ru => "https://asia.api.riotgames.com",
        Region.Oce => "https://asia.api.riotgames.com",
        Region.Tr => "https://tr.api.riotgames.com",
        Region.Lan => "https://americas.api.riotgames.com",
        Region.Las => "https://americas.api.riotgames.com",
        Region.Ph => "https://asia.api.riotgames.com",
        Region.Sg => "https://asia.api.riotgames.com",
        Region.Th => "https://asia.api.riotgames.com",
        Region.Tw => "https://asia.api.riotgames.com",
        Region.Vn => "https://asia.api.riotgames.com",
        Region.Me => "https://asia.api.riotgames.com",
        _ => throw new Exception("Region not found")
    };

    public static string GetRiotSummonerBaseUrl(Region region) => region switch
    {
        Region.Na => "https://na1.api.riotgames.com",
        Region.Euw => "https://euw1.api.riotgames.com",
        Region.Eun => "https://eun1.api.riotgames.com",
        Region.Kr => "https://kr.api.riotgames.com",
        Region.Br => "https://br1.api.riotgames.com",
        Region.Jp => "https://jp1.api.riotgames.com",
        Region.Ru => "https://ru.api.riotgames.com",
        Region.Oce => "https://oc1.api.riotgames.com",
        Region.Tr => "https://tr1.api.riotgames.com",
        Region.Lan => "https://la1.api.riotgames.com",
        Region.Las => "https://la2.api.riotgames.com",
        Region.Ph => "https://ph2.api.riotgames.com",
        Region.Sg => "https://sg2.api.riotgames.com",
        Region.Th => "https://th2.api.riotgames.com",
        Region.Tw => "https://tw2.api.riotgames.com",
        Region.Vn => "https://vn2.api.riotgames.com",
        Region.Me => "https://me1.api.riotgames.com",
        _ => throw new Exception("Region not found")
    };

    public static string GetLatestVersion()
    {
        var url = "https://ddragon.leagueoflegends.com/api/versions.json";
        var client = new HttpClient();
        var response = client.GetAsync(url).Result;
        var json = response.Content.ReadAsStringAsync().Result;
        var versions = JsonSerializer.Deserialize<List<string>>(json, JsonOptions);
        if (versions is null) throw new Exception("Failed to get versions");
        return versions[0] ?? throw new Exception("Failed to get latest version");
    }

    public static Region ParseRegion(string region) => region.ToLower() switch
    {
        "na" => Region.Na,
        "euw" => Region.Euw,
        "eun" => Region.Eun,
        "kr" => Region.Kr,
        "br" => Region.Br,
        "jp" => Region.Jp,
        "ru" => Region.Ru,
        "oce" => Region.Oce,
        "tr" => Region.Tr,
        "lan" => Region.Lan,
        "las" => Region.Las,
        "ph" => Region.Ph,
        "sg" => Region.Sg,
        "th" => Region.Th,
        "tw" => Region.Tw,
        "vn" => Region.Vn,
        "me" => Region.Me,
        _ => throw new Exception("Region not found")
    };
}
