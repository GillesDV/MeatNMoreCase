using Microsoft.EntityFrameworkCore;
using PriceService.Application;
using PriceService.Domain.Entity;

namespace PriceService.Infrastructure.Persistence;

public sealed class SqlPriceService(PriceDbContext dbContext) : IPriceRepository
{
    public ArticlePrice? GetByArticleId(int articleId)
    {
        return dbContext.ArticlePrices
            .AsNoTracking()
            .SingleOrDefault(articlePrice => articlePrice.ArticleId == articleId);
    }

    public ArticlePrice Create(ArticlePrice articlePrice)
    {
        dbContext.ArticlePrices.Add(articlePrice);
        dbContext.SaveChanges();

        return articlePrice;
    }

    public bool Update(int articleId, ArticlePrice articlePrice)
    {
        var existingArticlePrice = dbContext.ArticlePrices.Find(articleId);

        if (existingArticlePrice is null)
        {
            return false;
        }

        existingArticlePrice.BasicPriceInEuros = articlePrice.BasicPriceInEuros;

        dbContext.SaveChanges();

        return true;
    }
}
