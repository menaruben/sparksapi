using SparksApi.Api.Models;
using SparksApi.Api.Caches;
using SparksApi.Extensions;

namespace SparksApi.Api.Handlers.Runes;

public sealed class RunesApiClient(RuneCache runeCache) : IRunesApiClient
{
    private RuneCache RuneCache => runeCache;

    public IEnumerable<Rune> GetRunesFromPerks(Perks perks) => perks.ToRunes(RuneCache);
}