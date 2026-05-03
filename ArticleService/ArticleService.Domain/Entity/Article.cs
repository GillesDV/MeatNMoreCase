using ArticleService.Domain.Enums;

namespace ArticleService.Domain.Entity
{
    public class Article
    {
        public int ArticleId { get; set; }

        public string Omschrijving { get; set; } = string.Empty;

        public Eenheid Eenheid { get; set; } = Eenheid.Unknown;
    }
}
