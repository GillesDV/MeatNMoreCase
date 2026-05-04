namespace PriceService.Application.DTO
{
    public sealed class ArticlePriceDto
    {
        public int ArticleId { get; init; }

        public decimal TotalPriceInEuros { get; init; }
    }
}
