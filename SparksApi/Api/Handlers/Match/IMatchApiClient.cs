namespace SparksApi.Api.Handlers.Match;

public interface IMatchApiClient
{
    Task<Match[]> GetMatchesFromIds(IEnumerable<String> matchIds, Region region);
    Task<string[]> GetMatchIds(string puuid, Region region, int count, int skip);
    Task<MatchParticipation[]> GetMatchParticipations(string puuid, Region region, int count, int skip);
}
