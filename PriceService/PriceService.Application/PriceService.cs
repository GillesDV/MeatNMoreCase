using PriceService.Domain.Entity;

namespace PriceService.Application
{
    public class PriceService(IPriceRepository priceRepository) : IPriceService
    {
        public IReadOnlyCollection<ArticlePrice> GetAll()
        {
            return priceRepository.GetAll();
        }

        public ArticlePrice? GetByArticleId(int articleId)
        {
            return priceRepository.GetByArticleId(articleId);
        }

        public ArticlePrice Create(ArticlePrice articlePrice)
        {
            return priceRepository.Create(articlePrice);
        }

        public bool Update(int articleId, ArticlePrice articlePrice)
        {
            return priceRepository.Update(articleId, articlePrice);
        }

        public bool Delete(int articleId)
        {
            return priceRepository.Delete(articleId);
        }
    }
}
