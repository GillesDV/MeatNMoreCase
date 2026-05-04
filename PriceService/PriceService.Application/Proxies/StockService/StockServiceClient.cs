using System.Net;
using System.Net.Http.Json;

namespace PriceService.Application.Proxies.StockService;

public sealed class StockServiceClient(HttpClient httpClient) : IStockServiceClient
{
    public async Task<StockItemDto?> GetById(int articleId)
    {
        using var response = await httpClient.GetAsync($"stock-info/{articleId}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<StockItemDto>();
    }
}
