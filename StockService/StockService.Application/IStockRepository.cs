using StockService.Domain.Entity;

namespace StockService.Application
{
    public interface IStockRepository
    {
        IReadOnlyCollection<StockItem> GetAll();

        StockItem? GetByArticleId(int articleId);

        StockItem Create(StockItem stockInfo);

        bool Update(int articleId, StockItem stockInfo);

        bool Delete(int articleId);
    }
}
