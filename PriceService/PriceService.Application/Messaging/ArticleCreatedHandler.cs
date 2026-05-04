using BuildingBlocks.Contracts;
using NServiceBus;
using PriceService.Domain.Entity;

namespace PriceService.Application.Messaging;

public sealed class ArticleCreatedHandler(IPriceService priceService) : IHandleMessages<ArticleCreated>
{
    public Task Handle(ArticleCreated message, IMessageHandlerContext context)
    {
        if (priceService.GetByArticleId(message.ArticleId) is not null)
        {
            return Task.CompletedTask;
        }

        priceService.Create(new ArticlePrice
        {
            ArticleId = message.ArticleId,
            BasicPriceInEuros = 0m
        });

        return Task.CompletedTask;
    }
}
