using StockService.Domain.Entity;

namespace StockService.Application.Interfaces
{
    public interface IStockRepository
    {
        Task<StockItem?> GetByArticleId(int articleId);

        Task<StockItem> Create(StockItem stockInfo);

        Task<bool> Update(int articleId, StockItem stockInfo);
    }
}
