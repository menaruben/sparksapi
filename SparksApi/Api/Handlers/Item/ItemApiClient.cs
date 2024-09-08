using SparksApi.Api.Models;
using System.Text.Json;
using SparksApi.Api.Caches;

namespace SparksApi.Api.Handlers.Item;

public sealed class ItemApiClient : IItemApiClient
{
    private readonly HttpClient _client = new();
    private readonly ItemCache _itemDatabase;
    public static readonly int EmptyItemSlotId = 0;

    public ItemApiClient(ItemCache itemDatabase) {
        _itemDatabase = itemDatabase;
        LoadItems();
    }

    public Item GetItem(int itemId) =>
        _itemDatabase.TryGet(itemId) ?? throw new Exception($"Item with id {itemId} not found");

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
            try
            {
                _itemDatabase.TryAdd(item.Id, new Item(
                    item.Id,
                    item.Name,
                    item.Description,
                    item.Categories,
                    GetItemIconUrl(item.Id),
                    item.PriceTotal
                ));
            } catch (Exception e)
            {
                throw new Exception($"Failed to load item {item.Id}", e);
            }
        });
    }
}
