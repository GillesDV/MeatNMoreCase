using Microsoft.EntityFrameworkCore;
using StockService.Application;
using StockService.Domain.Entity;

namespace StockService.Infrastructure.Persistence;

public sealed class SqlStockService(StockDbContext dbContext) : IStockRepository
{
    public IReadOnlyCollection<StockInfo> GetAll()
    {
        return dbContext.StockInfos
            .AsNoTracking()
            .OrderBy(stockInfo => stockInfo.ArticleId)
            .ToArray();
    }

    public StockInfo? GetByArticleId(int articleId)
    {
        return dbContext.StockInfos
            .AsNoTracking()
            .SingleOrDefault(stockInfo => stockInfo.ArticleId == articleId);
    }

    public StockInfo Create(StockInfo stockInfo)
    {
        dbContext.StockInfos.Add(stockInfo);
        dbContext.SaveChanges();

        return stockInfo;
    }

    public bool Update(int articleId, StockInfo stockInfo)
    {
        var existingStockInfo = dbContext.StockInfos.Find(articleId);

        if (existingStockInfo is null)
        {
            return false;
        }

        existingStockInfo.Voorraad = stockInfo.Voorraad;
        existingStockInfo.Locatie = stockInfo.Locatie;

        dbContext.SaveChanges();

        return true;
    }

    public bool Delete(int articleId)
    {
        var existingStockInfo = dbContext.StockInfos.Find(articleId);

        if (existingStockInfo is null)
        {
            return false;
        }

        dbContext.StockInfos.Remove(existingStockInfo);
        dbContext.SaveChanges();

        return true;
    }
}
