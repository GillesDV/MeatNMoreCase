using ArticleService.Domain.Enums;

namespace ArticleService.Domain.Entity
{
    public class Article
    {
        public int ArticleId { get; set; }

        public string Description { get; set; } = string.Empty;

        public ArticleUnit Unit { get; set; } = ArticleUnit.Unknown;
    }
}
