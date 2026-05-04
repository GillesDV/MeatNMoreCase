using ArticleService.Domain.Entity;

namespace ArticleService.Application
{
    public interface IArticleService
    {
        IReadOnlyCollection<Article> GetAll();

        Article? GetById(int articleId);

        Task<Article> Create(Article article);

        bool Update(int articleId, Article article);

        bool Delete(int articleId);
    }
}
