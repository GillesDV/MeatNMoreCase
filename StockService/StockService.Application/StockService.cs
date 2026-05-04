using StockService.Application.DTO;
using StockService.Domain.Entity;

namespace StockService.Application
{
    public class StockService(IStockRepository stockRepository) : IStockService
    {
        public StockItem? GetByArticleId(int articleId)
        {
            return stockRepository.GetByArticleId(articleId);
        }

        public StockItem Create(StockItem stockInfo)
        {
            return stockRepository.Create(stockInfo);
        }

        public bool Update(int articleId, UpdateStockItemDto stockInfo)
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
