using StockService.Domain.Entity;

namespace StockService.Application
{
    public interface IStockRepository
    {
        IReadOnlyCollection<StockInfo> GetAll();

        StockInfo? GetByArticleId(int articleId);

        StockInfo Create(StockInfo stockInfo);

        bool Update(int articleId, StockInfo stockInfo);

        bool Delete(int articleId);
    }
}
