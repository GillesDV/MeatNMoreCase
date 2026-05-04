using Microsoft.EntityFrameworkCore;
using PriceService.Application.Interfaces;
using PriceService.Domain.Entity;

namespace PriceService.Infrastructure.Persistence;

public sealed class SqlPriceService(PriceDbContext dbContext) : IPriceRepository
{
    public Task<ArticlePrice?> GetByArticleId(int articleId)
    {
        return dbContext.ArticlePrices
            .AsNoTracking()
            .SingleOrDefaultAsync(articlePrice => articlePrice.ArticleId == articleId);
    }

    public async Task<ArticlePrice> Create(ArticlePrice articlePrice)
    {
        await dbContext.ArticlePrices.AddAsync(articlePrice);
        await dbContext.SaveChangesAsync();

        return articlePrice;
    }

    public async Task<bool> Update(int articleId, ArticlePrice articlePrice)
    {
        var existingArticlePrice = await dbContext.ArticlePrices.FindAsync(articleId);

        if (existingArticlePrice is null)
        {
            return false;
        }

        existingArticlePrice.BasicPriceInEuros = articlePrice.BasicPriceInEuros;

        await dbContext.SaveChangesAsync();

        return true;
    }
}
