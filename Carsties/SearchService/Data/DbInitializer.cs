using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;

namespace SearchService;

public static class DbInitializer
{
    private const string Path = "Data/auctions.json";

    public static async Task InitMongoDb(this WebApplication app)
    {
        string connectionString = app.Configuration.GetConnectionString("MongoDbConnectionTest");
        MongoClientSettings settings = MongoClientSettings.FromConnectionString(connectionString);
        await DB.InitAsync("SearchDb", settings);

        await DB.Index<Item>()
            .Key(x => x.Make, KeyType.Text)
            .Key(x => x.Model, KeyType.Text)
            .Key(x => x.Color, KeyType.Text)
            .CreateAsync();

        await SeedMongoDbFromJsonFile();

        await app.GetItems();

    }

    private static async Task GetItems(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        AuctionSvcHttpClient httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();

        List<Item> items = await httpClient.GetItemsForSearchDb();

        Console.WriteLine(items.Count + " returned from the auction service");

        if (items.Count > 0) await DB.SaveAsync(items);
    }

    private static async Task SeedMongoDbFromJsonFile()
    {
        var count = await DB.CountAsync<Item>();

        if (count == 0)
        {
            string itemData = await File.ReadAllTextAsync(Path);

            JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var items = JsonSerializer.Deserialize<List<Item>>(itemData, options);

            await DB.SaveAsync(items);
        }
    }
}
