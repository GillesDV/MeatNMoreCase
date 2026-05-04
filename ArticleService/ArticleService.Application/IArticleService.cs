using ArticleService.Domain.Entity;

namespace ArticleService.Application
{
    public interface IArticleService
    {
        Task<IReadOnlyCollection<Article>> GetAll();

        Task<Article?> GetById(int articleId);

        Task<Article> Create(Article article);

        Task<bool> Update(int articleId, Article article);

        Task<bool> Delete(int articleId);
    }
}
