using BuildingBlocks.Contracts;
using NServiceBus;
using PriceService.Application.Interfaces;
using PriceService.Domain.Entity;

namespace PriceService.Application.Messaging;

public sealed class ArticleCreatedHandler(IPriceService priceService) : IHandleMessages<ArticleCreated>
{
    public async Task Handle(ArticleCreated message, IMessageHandlerContext context)
    {
        if (await priceService.GetByArticleId(message.ArticleId) is not null)
        {
            return;
        }

        await priceService.Create(new ArticlePrice
        {
            ArticleId = message.ArticleId,
            BasicPriceInEuros = 0m
        });
    }
}
