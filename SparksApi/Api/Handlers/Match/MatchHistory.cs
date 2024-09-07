using System.Collections;

namespace SparksApi.Api.Handlers.Match;

public sealed class MatchHistory : IEnumerable<MatchParticipation>
{
    private readonly List<MatchParticipation> _matchParticipations = new();

    private MatchHistory(IEnumerable<MatchParticipation> matchParticipations) =>
        _matchParticipations.AddRange(matchParticipations);

    public static MatchHistory From(IEnumerable<MatchParticipation> ms) => new(ms);
    public static MatchHistory Empty() => new(new List<MatchParticipation>());

    public MatchHistory AddRange(IEnumerable<MatchParticipation> matchParticipations) =>
        new(_matchParticipations.Concat(matchParticipations));

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<MatchParticipation> GetEnumerator() => _matchParticipations.GetEnumerator();
}