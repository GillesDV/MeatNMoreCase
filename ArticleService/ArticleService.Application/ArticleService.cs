using ArticleService.Domain.Entity;

namespace ArticleService.Application
{
    public class ArticleService(IArticleRepository articleRepository) : IArticleService
    {
        public IReadOnlyCollection<Article> GetAll()
        {
            return articleRepository.GetAll();
        }

        public Article? GetById(int articleId)
        {
            return articleRepository.GetById(articleId);
        }

        public Article Create(Article article)
        {
            return articleRepository.Create(article);
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
