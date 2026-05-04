using StockService.Domain.Entity;

namespace StockService.Application
{
    public class StockService(IStockRepository stockRepository) : IStockService
    {
        public IReadOnlyCollection<StockItem> GetAll()
        {
            return stockRepository.GetAll();
        }

        public StockItem? GetByArticleId(int articleId)
        {
            return stockRepository.GetByArticleId(articleId);
        }

        public StockItem Create(StockItem stockInfo)
        {
            return stockRepository.Create(stockInfo);
        }

        public bool Update(int articleId, StockItem stockInfo)
        {
            return stockRepository.Update(articleId, stockInfo);
        }

        public bool Delete(int articleId)
        {
            return stockRepository.Delete(articleId);
        }
    }
}
