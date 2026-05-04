using ArticleService.Domain.Entity;

namespace ArticleService.Application
{
    public class ArticleService(
        IArticleRepository articleRepository,
        IArticleCreatedEventPublisher articleCreatedEventPublisher) : IArticleService
    {
        public IReadOnlyCollection<Article> GetAll()
        {
            return articleRepository.GetAll();
        }

        public Article? GetById(int articleId)
        {
            return articleRepository.GetById(articleId);
        }

        public async Task<Article> Create(Article article)
        {
            Article createdArticle = articleRepository.Create(article);

            await articleCreatedEventPublisher.PublishAsync(createdArticle);

            return createdArticle;
        }

        public bool Update(int articleId, Article article)
        {
            return articleRepository.Update(articleId, article);
        }

        public bool Delete(int articleId)
        {
            return articleRepository.Delete(articleId);
        }
    }
}
