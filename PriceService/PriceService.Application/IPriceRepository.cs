using PriceService.Domain.Entity;

namespace PriceService.Application
{
    public interface IPriceRepository
    {
        Task<ArticlePrice?> GetByArticleId(int articleId);

        Task<ArticlePrice> Create(ArticlePrice articlePrice);

        Task<bool> Update(int articleId, ArticlePrice articlePrice);
    }
}
