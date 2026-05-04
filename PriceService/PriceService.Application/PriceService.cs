using PriceService.Application.DTO;
using PriceService.Application.Interfaces;
using PriceService.Application.Proxies.ArticleService;
using PriceService.Application.Proxies.StockService;
using PriceService.Domain.Entity;

namespace PriceService.Application
{
    public class PriceService(
        IPriceRepository priceRepository,
        IArticleServiceClient articleServiceClient,
        IStockServiceClient stockServiceClient) : IPriceService
    {
        public Task<ArticlePrice> Create(ArticlePrice articlePrice)
        {
            return priceRepository.Create(articlePrice);
        }

        public async Task<ArticlePriceDto?> GetByArticleId(int articleId)
        {
            var articlePrice = await priceRepository.GetByArticleId(articleId);

            if (articlePrice is null)
            {
                return null;
            }

            var article = await articleServiceClient.GetById(articleId);
            var stock = await stockServiceClient.GetById(articleId);

            if (article is null || stock is null)
            {
                return null;
            }

            var quantityInKg = stock.Quantity; //TODO is this correct? Unsure, but don't see another option. If so, simplify the 2 params ofc

            return new ArticlePriceDto
            {
                ArticleId = articlePrice.ArticleId,
                TotalPriceInEuros = articlePrice.CalculateTotalPrice(quantityInKg, stock.Quantity, article.Unit)
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
