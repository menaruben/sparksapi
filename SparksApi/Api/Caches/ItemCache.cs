using System.Collections.Concurrent;
using SparksApi.Api.Handlers.Item;

namespace SparksApi.Api.Caches;

public class ItemCache : ICache<int, Item> {
    private readonly ConcurrentDictionary<int, Item> _itemCache = new();
    
    public Item? TryGet(int id) => _itemCache.TryGetValue(id, out var item) ? item : null;
    public bool TryAdd(int id, Item item) => _itemCache.TryAdd(item.Id, item);
}