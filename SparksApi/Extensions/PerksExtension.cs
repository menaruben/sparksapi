using SparksApi.Api.Caches;
using SparksApi.Api.Handlers.Runes;
using SparksApi.Api.Models;

namespace SparksApi.Extensions;

public static class PerksExtension {
    public static IEnumerable<Rune> ToRunes(this Perks perks, RuneCache runeCache) =>
        perks.Styles
            .SelectMany(style => style.Selections)
            .Select(selection => runeCache.TryGet(selection.Perk))
            .Where(rune => rune != null)
            .Select(rune => rune!);
}