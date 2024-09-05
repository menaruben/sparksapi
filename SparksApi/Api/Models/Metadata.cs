namespace SparksApi.Api.Models;

public sealed record Metadata(
    string DataVersion,
    string MatchId,
    String[] Participants       // contains Participant PUUIDs
);