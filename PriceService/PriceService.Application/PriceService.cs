using PriceService.Domain.Entity;

namespace PriceService.Application
{
    public class PriceService(IPriceRepository priceRepository) : IPriceService
    {
        public ArticlePrice Create(ArticlePrice articlePrice)
        {
            return priceRepository.Create(articlePrice);
        }

        public ArticlePrice? GetByArticleId(int articleId)
        {
            var articlePrice = priceRepository.GetByArticleId(articleId);

            //TODO  get these values from a GET call from StockService and ArtikelsErvice
            var quantityInKg = 15;
            var stockInKg = 15;

            if (articlePrice is null)
            {
                return null;
            }

            //TODO use a separate DTO
            return new ArticlePrice
            {
                ArticleId = articlePrice.ArticleId,
                BasicPriceInEuros = articlePrice.CalculateTotalPrice(quantityInKg, stockInKg)
            };
        }

        public bool Update(int articleId, ArticlePrice articlePrice)
        {
            return priceRepository.Update(articleId, articlePrice);
        }
    }
}
