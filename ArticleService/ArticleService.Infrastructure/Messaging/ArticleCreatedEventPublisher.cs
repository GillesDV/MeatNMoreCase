using ArticleService.Application.Interfaces;
using ArticleService.Domain.Entity;
using BuildingBlocks.Contracts;
using NServiceBus;

namespace ArticleService.Infrastructure.Messaging;

public sealed class ArticleCreatedEventPublisher(IMessageSession messageSession) : IArticleCreatedEventPublisher
{
    public Task PublishAsync(Article article)
    {
        return messageSession.Publish(new ArticleCreated(
            article.ArticleId));
    }
}
