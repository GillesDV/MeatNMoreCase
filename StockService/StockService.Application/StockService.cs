using StockService.Domain.Entity;

namespace StockService.Application
{
    public class StockService(IStockRepository stockRepository) : IStockService
    {
        public IReadOnlyCollection<StockInfo> GetAll()
        {
            return stockRepository.GetAll();
        }

        public StockInfo? GetByArticleId(int articleId)
        {
            return stockRepository.GetByArticleId(articleId);
        }

        public StockInfo Create(StockInfo stockInfo)
        {
            return stockRepository.Create(stockInfo);
        }

        public bool Update(int articleId, StockInfo stockInfo)
        {
            return stockRepository.Update(articleId, stockInfo);
        }

        public bool Delete(int articleId)
        {
            return stockRepository.Delete(articleId);
        }
    }
}
