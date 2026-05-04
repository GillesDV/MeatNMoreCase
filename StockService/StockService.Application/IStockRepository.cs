using StockService.Domain.Entity;

namespace StockService.Application
{
    public interface IStockRepository
    {
        StockItem? GetByArticleId(int articleId);

        StockItem Create(StockItem stockInfo);

        bool Update(int articleId, StockItem stockInfo);
    }
}
