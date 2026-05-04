using StockService.Application.DTO;
using StockService.Domain.Entity;

namespace StockService.Application
{
    public interface IStockService
    {
        StockItem? GetByArticleId(int articleId);

        StockItem Create(StockItem stockInfo);

        bool Update(int articleId, UpdateStockItemDto stockInfo);
    }
}
