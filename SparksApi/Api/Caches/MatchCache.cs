using SparksApi.Api.Caches;
using SparksApi.Api.Handlers.Match;
using System.Collections.Concurrent;

public class MatchCache : ICache<string, Match>
{
    private readonly ConcurrentDictionary<string, Match> _matchCache = new();

    public Match? TryGet(string key) => _matchCache.TryGetValue(key, out var match) ? match : null;
    public bool TryAdd(string key, Match value) => _matchCache.TryAdd(key, value);
}