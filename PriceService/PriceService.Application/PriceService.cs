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

        public async Task<ArticlePriceDto?> GetByArticleId(int articleId, int? quantityOrdered = null)
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

            return new ArticlePriceDto
            {
                ArticleId = articlePrice.ArticleId,
                TotalPriceInEuros = articlePrice.CalculateTotalPrice(quantityOrdered ?? 1, stock.Quantity, article.Unit)
            };
        }

        public async Task<ArticlePriceBreakdownDto?> GetPriceBreakdownByArticleId(int articleId)
        {
            var articlePrice = await priceRepository.GetByArticleId(articleId);

            if (articlePrice is null)
            {
                return null;
            }

            var article = await articleServiceClient.GetById(articleId);

            if (article is null)
            {
                return null;
            }

            var breakdown = articlePrice.CalculatePriceBreakdown(article.Unit);

            return new ArticlePriceBreakdownDto
            {
                ArticleId = breakdown.ArticleId,
                Unit = breakdown.Unit,
                DefaultUnitPriceInEuros = breakdown.DefaultUnitPriceInEuros,
                PriceTiers = breakdown.PriceTiers
                    .Select(tier => new ArticlePriceTierDto
                    {
                        MinimumQuantity = tier.MinimumQuantity,
                        UnitPriceInEuros = tier.UnitPriceInEuros,
                        ReductionPercentage = tier.ReductionPercentage,
                    })
                    .ToArray(),
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
