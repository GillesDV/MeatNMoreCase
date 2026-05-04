namespace PriceService.Application.Proxies.StockService;

public sealed class StockItemDto
{
    public int ArticleId { get; init; }

    public int Quantity { get; init; }

    public int Location { get; init; }
}
