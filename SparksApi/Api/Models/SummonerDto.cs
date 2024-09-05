namespace SparksApi.Api.Models;

public sealed record SummonerDto(
    string Id,
    string AccountId,
    string Puuid,
    int ProfileIconId,
    long RevisionDate,
    long SummonerLevel
);