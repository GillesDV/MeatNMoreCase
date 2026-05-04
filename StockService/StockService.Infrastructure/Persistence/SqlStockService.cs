using Microsoft.EntityFrameworkCore;
using StockService.Application.Interfaces;
using StockService.Domain.Entity;

namespace StockService.Infrastructure.Persistence;

public sealed class SqlStockService(StockDbContext dbContext) : IStockRepository
{
    public Task<StockItem?> GetByArticleId(int articleId)
    {
        return dbContext.StockInfos
            .AsNoTracking()
            .SingleOrDefaultAsync(stockInfo => stockInfo.ArticleId == articleId);
    }

    public async Task<StockItem> Create(StockItem stockInfo)
    {
        await dbContext.StockInfos.AddAsync(stockInfo);
        await dbContext.SaveChangesAsync();

        return stockInfo;
    }

    public async Task<bool> Update(int articleId, StockItem stockInfo)
    {
        var existingStockInfo = await dbContext.StockInfos.FindAsync(articleId);

        if (existingStockInfo is null)
        {
            return false;
        }

        existingStockInfo.Quantity = stockInfo.Quantity;
        existingStockInfo.Location = stockInfo.Location;

        await dbContext.SaveChangesAsync();

        return true;
    }
}
