using StockService.Application.DTO;
using StockService.Domain.Entity;

namespace StockService.Application.Interfaces
{
    public interface IStockService
    {
        Task<StockItem?> GetByArticleId(int articleId);

        Task<StockItem> Create(StockItem stockInfo);

        Task<bool> Update(int articleId, UpdateStockItemDto stockInfo);
    }
}
