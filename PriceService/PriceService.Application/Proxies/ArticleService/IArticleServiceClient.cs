namespace PriceService.Application.Proxies.ArticleService;

public interface IArticleServiceClient
{
    Task<ArticleDto?> GetById(int articleId);
}
