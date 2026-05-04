using StockService.Domain.Enums;

namespace StockService.Domain.Entity
{
    public class StockItem
    {
        public int ArticleId { get; set; }

        public int Quantity { get; set; } 

        public StockLocation Location { get; set; } = StockLocation.Unknown; 
    }
}
