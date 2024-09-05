namespace SparksApi.Api.Handlers.Match;

public interface IMatchApiClient
{
    Task<MatchCollection> GetMatchesFromIds(IEnumerable<String> matchIds, Region region);
    Task<IEnumerable<String>> GetMatchIds(string puuid, Region region, int count, int skip);
}
