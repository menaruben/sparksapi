using SparksApi.Api.Models;

namespace SparksApi.Api.Handlers.Runes;

public interface IRunesApiClient {
    IEnumerable<Rune> GetRunesFromPerks(Perks perks);
}