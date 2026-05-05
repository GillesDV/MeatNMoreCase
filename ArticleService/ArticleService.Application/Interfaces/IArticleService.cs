using ArticleService.Domain.Entity;

namespace ArticleService.Application.Interfaces
{
    public interface IArticleService
    {
        Task<IReadOnlyCollection<Article>> GetAll();

        Task<Article?> GetById(int articleId);

        Task<Article> Create(Article article);

        Task<bool> Update(int articleId, Article article);
    }
}
