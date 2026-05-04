namespace PriceService.Application.Proxies.StockService;

public interface IStockServiceClient
{
    Task<StockItemDto?> GetById(int articleId);
}
