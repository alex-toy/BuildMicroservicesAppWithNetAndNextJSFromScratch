using MongoDB.Driver;
using MongoDB.Entities;
using System.Linq.Expressions;

namespace SearchService;

public class AuctionSvcHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public AuctionSvcHttpClient(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<List<Item>> GetItemsForSearchDb()
    {
        Func<SortDefinitionBuilder<Item>, SortDefinition<Item>> byUpdateDate = x => x.Descending(x => x.UpdatedAt);

        Expression<Func<Item, string>> udpdate = x => x.UpdatedAt.ToString();

        string lastUpdated = await DB.Find<Item, string>().Sort(byUpdateDate).Project(udpdate).ExecuteFirstAsync();

        string requestUri = _config["AuctionServiceUrl"] + "/api/auctions?date=" + lastUpdated;

        return await _httpClient.GetFromJsonAsync<List<Item>>(requestUri);
    }
}
