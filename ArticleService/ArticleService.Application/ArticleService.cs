using System.Collections.Concurrent;
using ArticleService.Domain.Entity;

namespace ArticleService.Application
{
    public class ArticleService : IArticleService
    {
        private readonly ConcurrentDictionary<int, Article> articles = new();
        private int nextArticleId;

        public IReadOnlyCollection<Article> GetAll()
        {
            return articles.Values
                .OrderBy(article => article.ArticleId)
                .ToArray();
        }

        public Article? GetById(int articleId)
        {
            articles.TryGetValue(articleId, out var article);

            return article;
        }

        public Article Create(Article article)
        {
            if (article.ArticleId <= 0)
            {
                article.ArticleId = Interlocked.Increment(ref nextArticleId);
            }

            articles[article.ArticleId] = article;

            return article;
        }

        public bool Update(int articleId, Article article)
        {
            if (!articles.ContainsKey(articleId))
            {
                return false;
            }

            article.ArticleId = articleId;
            articles[articleId] = article;

            return true;
        }

        public bool Delete(int articleId)
        {
            return articles.TryRemove(articleId, out _);
        }
    }
}
