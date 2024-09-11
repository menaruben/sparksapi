using SparksApi.Api.Caches;

namespace SparksApi.Api.Handlers.Item;

public sealed class ItemApiClient(ItemCache itemCache) : IItemApiClient
{
    private ItemCache ItemCache => itemCache;
    public static readonly int EmptyItemSlotId = 0;

    public Item GetItem(int itemId) =>
        ItemCache.TryGet(itemId)
        ?? throw new Exception($"Item with id {itemId} not found");
}
