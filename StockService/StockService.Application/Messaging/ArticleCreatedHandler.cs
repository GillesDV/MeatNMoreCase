using BuildingBlocks.Contracts;
using NServiceBus;
using StockService.Application.Interfaces;
using StockService.Domain.Entity;
using StockService.Domain.Enums;

namespace StockService.Application.Messaging;

public sealed class ArticleCreatedHandler(IStockService stockService) : IHandleMessages<ArticleCreated>
{
    public async Task Handle(ArticleCreated message, IMessageHandlerContext context)
    {
        if (await stockService.GetByArticleId(message.ArticleId) is not null)
        {
            return;
        }

        await stockService.Create(new StockItem
        {
            ArticleId = message.ArticleId,
            Quantity = 0,
            Location = StockLocation.MainWharehouse
        });
    }
}
