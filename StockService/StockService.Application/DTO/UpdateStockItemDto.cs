using StockService.Domain.Enums;

namespace StockService.Application.DTO
{
    public sealed class UpdateStockItemDto
    {
        public int Quantity { get; init; }

        public StockLocation Location { get; init; } = StockLocation.Unknown;
    }
}
