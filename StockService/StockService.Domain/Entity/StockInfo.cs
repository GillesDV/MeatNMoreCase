using StockService.Domain.Enums;

namespace StockService.Domain.Entity
{
    public class StockInfo
    {
        public int ArticleId { get; set; }

        public int Voorraad { get; set; }

        public Locatie Locatie { get; set; } = Locatie.Unknown;
    }
}
