using PriceService.Application.DTO;
using PriceService.Domain.Entity;

namespace PriceService.Application
{
    public interface IPriceService
    {
        ArticlePriceDto? GetByArticleId(int articleId);

        ArticlePrice Create(ArticlePrice articlePrice);

        bool Update(int articleId, ArticlePrice articlePrice);
    }
}
