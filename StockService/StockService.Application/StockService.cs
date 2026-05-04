using StockService.Application.DTO;
using StockService.Application.Interfaces;
using StockService.Domain.Entity;

namespace StockService.Application
{
    public class StockService(IStockRepository stockRepository) : IStockService
    {
        public Task<StockItem?> GetByArticleId(int articleId)
        {
            return stockRepository.GetByArticleId(articleId);
        }

        public Task<StockItem> Create(StockItem stockInfo)
        {
            return stockRepository.Create(stockInfo);
        }

        public Task<bool> Update(int articleId, UpdateStockItemDto stockInfo)
        {
            return stockRepository.Update(articleId, new StockItem
            {
                ArticleId = articleId,
                Quantity = stockInfo.Quantity,
                Location = stockInfo.Location
            });
        }
    }
}
