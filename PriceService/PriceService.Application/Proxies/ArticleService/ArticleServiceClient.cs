using System.Net;
using System.Net.Http.Json;

namespace PriceService.Application.Proxies.ArticleService;

public sealed class ArticleServiceClient(HttpClient httpClient) : IArticleServiceClient
{
    public async Task<ArticleDto?> GetById(int articleId)
    {
        using var response = await httpClient.GetAsync($"articles/{articleId}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ArticleDto>();
    }
}
