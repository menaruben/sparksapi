using System.Collections;

namespace SparksApi.Api.Handlers.Match;

public sealed class MatchCollection : IEnumerable<Match>
{
    private readonly IEnumerable<Match> _matches;

    private MatchCollection() =>
        _matches = new List<Match>();

    private MatchCollection(Match match) =>
        _matches = new List<Match> { match };

    private MatchCollection(IEnumerable<Match> matches) =>
        _matches = matches;
    
    public static MatchCollection From(IEnumerable<Match> matches) => new(matches);

    public static MatchCollection Empty() => new();
    public MatchCollection Add(Match match) => new(_matches.Append(match));
    public MatchCollection AddRange(IEnumerable<Match> matches) => new(_matches.Concat(matches));

    public static implicit operator MatchCollection(Match match) => new(match);
    public static implicit operator MatchCollection(List<Match> matches) => new(matches);

    public IEnumerator<Match> GetEnumerator() => _matches.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
