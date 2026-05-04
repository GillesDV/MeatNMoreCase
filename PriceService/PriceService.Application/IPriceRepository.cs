using PriceService.Domain.Entity;

namespace PriceService.Application
{
    public interface IPriceRepository
    {
        ArticlePrice? GetByArticleId(int articleId);

        ArticlePrice Create(ArticlePrice articlePrice);

        bool Update(int articleId, ArticlePrice articlePrice);
    }
}
