using PriceService.Application.DTO;
using PriceService.Domain.Entity;

namespace PriceService.Application.Interfaces
{
    public interface IPriceService
    {
        Task<ArticlePriceDto?> GetByArticleId(int articleId);

        Task<ArticlePrice> Create(ArticlePrice articlePrice);

        Task<bool> Update(int articleId, UpdateArticlePriceDto articlePrice);
    }
}
