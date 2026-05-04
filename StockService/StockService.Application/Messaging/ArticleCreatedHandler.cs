using BuildingBlocks.Contracts;
using NServiceBus;
using StockService.Domain.Entity;
using StockService.Domain.Enums;

namespace StockService.Application.Messaging;

public sealed class ArticleCreatedHandler(IStockService stockService) : IHandleMessages<ArticleCreated>
{
    public Task Handle(ArticleCreated message, IMessageHandlerContext context)
    {
        if (stockService.GetByArticleId(message.ArticleId) is not null)
        {
            return Task.CompletedTask;
        }

        stockService.Create(new StockItem
        {
            ArticleId = message.ArticleId,
            Quantity = 0,
            Location = StockLocation.MainWharehouse
        });

        return Task.CompletedTask;
    }
}
