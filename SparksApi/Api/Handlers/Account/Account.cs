namespace SparksApi.Api.Handlers.Account;

public sealed record Account(
    string Puuid,
    string GameName,
    string TagLine,
    Region Region
);