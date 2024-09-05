namespace SparksApi.Api.Models;

public sealed record Info(
    string EndOfGameResult,
    long GameCreation,
    long GameDuration,
    long GameId,
    string GameMode,
    string GameName,
    long GameStartTimeStamp,
    string GameType,
    string GameVersion,
    int MapId,
    Participant[] Participants,
    string PlatformId,
    int QueueId,
    Team[] Teams,
    string TournamentCode
);