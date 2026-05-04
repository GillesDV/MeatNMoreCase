using ArticleService.Domain.Entity;

namespace ArticleService.Application;

public interface IArticleCreatedEventPublisher
{
    Task PublishAsync(Article article);
}
