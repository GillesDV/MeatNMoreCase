using ArticleService.Domain.Entity;

namespace ArticleService.Application
{
    public class ArticleService(
        IArticleRepository articleRepository,
        IArticleCreatedEventPublisher articleCreatedEventPublisher) : IArticleService
    {
        public Task<IReadOnlyCollection<Article>> GetAll()
        {
            return articleRepository.GetAll();
        }

        public Task<Article?> GetById(int articleId)
        {
            return articleRepository.GetById(articleId);
        }

        public async Task<Article> Create(Article article)
        {
            Article createdArticle = await articleRepository.Create(article);

            await articleCreatedEventPublisher.PublishAsync(createdArticle);

            return createdArticle;
        }

        public Task<bool> Update(int articleId, Article article)
        {
            return articleRepository.Update(articleId, article);
        }

        public Task<bool> Delete(int articleId)
        {
            return articleRepository.Delete(articleId);
        }
    }
}
