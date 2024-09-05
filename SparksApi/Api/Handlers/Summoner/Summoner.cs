namespace SparksApi.Api.Handlers.Summoner;

public sealed record Summoner(
    string AccountId,
    string ProfileIconUrl,
    long RevisionDate,
    string Id,
    string Puuid,
    long SummonerLevel
);