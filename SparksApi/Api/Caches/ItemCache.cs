using System.Collections.Concurrent;
using System.Text.Json;
using SparksApi.Api.Handlers.Item;
using SparksApi.Api.Models;

namespace SparksApi.Api.Caches;

public class ItemCache : ICache<int, Item> {
    private readonly ConcurrentDictionary<int, Item> _itemCache = new();
    private readonly HttpClient _client;
    private readonly ILogger<ItemCache> _logger;

    public ItemCache(HttpClient httpClient, ILogger<ItemCache> logger) 
    {
        _client = httpClient;
        _logger = logger;
        LoadItems().Wait();
    }
    
    public Item? TryGet(int id) => _itemCache.TryGetValue(id, out var item) ? item : null;
    public bool TryAdd(int id, Item item) => _itemCache.TryAdd(item.Id, item);

    private async Task LoadItems() {
        var url = $"{ApiHelper.CdragonBaseUrl}/plugins/rcp-be-lol-game-data/global/en_gb/v1/items.json";
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        _logger.LogInformation("Successfully loaded json from response");
        var items = JsonSerializer.Deserialize<ItemDto[]>(json, ApiHelper.JsonOptions);
        if (items is null) throw new Exception("Could not load items");
        _logger.LogInformation($"Successfully deserialized {items.Length} items");
        Parallel.ForEach(items, item =>
        {
            TryAdd(item.Id, new Item(
                item.Id,
                item.Name,
                item.Description,
                item.Categories,
                GetItemIconUrl(item.Id),
                item.PriceTotal
            ));
        });

        _logger.LogInformation("Successfully loaded items into cache");
    }

    private string GetItemIconUrl(int itemId) =>
        $"{ApiHelper.DdragonBaseUrl}/{ApiHelper.GetLatestVersion()}" +
        $"/img/item/{itemId}.png";
}