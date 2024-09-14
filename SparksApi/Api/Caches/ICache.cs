namespace SparksApi.Api.Caches;

public interface ICache<TKey, TValue>
{
    TValue? TryGet(TKey key);
    bool TryAdd(TKey key, TValue value);
}