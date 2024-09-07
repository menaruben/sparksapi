using SparksApi.Api.Models;
using System.Collections.Concurrent;
using System.Text.Json;

namespace SparksApi.Api.Handlers.Item;

public sealed class ItemApiClient : IItemApiClient
{
    private readonly HttpClient _client = new();
    private readonly ConcurrentDictionary<int, Item> _itemCache = new();
    public static readonly int EmptyItemSlotId = 0;

    public ItemApiClient() { LoadItems(); }

    public Item GetItem(int itemId) =>
        _itemCache.TryGetValue(itemId, out var item)
            ? item
            : throw new Exception($"Item with id {itemId} not found");

    private string GetItemIconUrl(int itemId) =>
        $"{ApiHelper.DdragonBaseUrl}/{ApiHelper.GetLatestVersion()}" +
        $"/img/item/{itemId}.png";

    private void LoadItems()
    {
        var url = $"{ApiHelper.CdragonBaseUrl}/plugins/rcp-be-lol-game-data/global/en_gb/v1/items.json";
        var response = _client.GetAsync(url).Result;
        var json = response.Content.ReadAsStringAsync().Result;
        var items = JsonSerializer.Deserialize<IEnumerable<ItemDto>>(json, ApiHelper.JsonOptions);
        if (items is null) throw new Exception("Could not load items");
        Parallel.ForEach(items, item =>
        {
            _itemCache[item.Id] = new Item(
                item.Id,
                item.Name,
                item.Description,
                item.Categories,
                GetItemIconUrl(item.Id),
                item.PriceTotal
            );
        });
    }
}
