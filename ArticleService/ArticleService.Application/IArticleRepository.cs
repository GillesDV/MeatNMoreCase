using ArticleService.Domain.Entity;

namespace ArticleService.Application
{
    public interface IArticleRepository
    {
        IReadOnlyCollection<Article> GetAll();

        Article? GetById(int articleId);

        Article Create(Article article);

        bool Update(int articleId, Article article);

        bool Delete(int articleId);
    }
}
