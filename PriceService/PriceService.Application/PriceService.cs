using PriceService.Application.DTO;
using PriceService.Domain.Entity;

namespace PriceService.Application
{
    public class PriceService(IPriceRepository priceRepository) : IPriceService
    {
        public Task<ArticlePrice> Create(ArticlePrice articlePrice)
        {
            return priceRepository.Create(articlePrice);
        }

        public async Task<ArticlePriceDto?> GetByArticleId(int articleId)
        {
            var articlePrice = await priceRepository.GetByArticleId(articleId);

            //TODO  get these values from a GET call from StockService and ArtikelsErvice
            var quantityInKg = 15;
            var stockInKg = 15;

            if (articlePrice is null)
            {
                return null;
            }

            return new ArticlePriceDto
            {
                ArticleId = articlePrice.ArticleId,
                TotalPriceInEuros = articlePrice.CalculateTotalPrice(quantityInKg, stockInKg)
            };
        }

        public Task<bool> Update(int articleId, UpdateArticlePriceDto articlePrice)
        {
            return priceRepository.Update(articleId, new ArticlePrice
            {
                ArticleId = articleId,
                BasicPriceInEuros = articlePrice.BasicPriceInEuros
            });
        }
    }
}
