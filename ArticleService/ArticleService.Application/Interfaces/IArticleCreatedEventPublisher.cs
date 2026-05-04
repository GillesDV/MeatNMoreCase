using ArticleService.Domain.Entity;

namespace ArticleService.Application.Interfaces;

public interface IArticleCreatedEventPublisher
{
    Task PublishAsync(Article article);
}
