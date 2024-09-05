using SparksApi.Api.Models;

namespace SparksApi.Api.Handlers.Match;

public sealed record Match(
    string MatchId,
    string[] ParticipantPuuids,
    Participant[] Participants,
    string GameMode
);