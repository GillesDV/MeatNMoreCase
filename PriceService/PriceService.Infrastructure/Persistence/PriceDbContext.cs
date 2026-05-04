using Microsoft.EntityFrameworkCore;
using PriceService.Domain.Entity;

namespace PriceService.Infrastructure.Persistence;

public sealed class PriceDbContext(DbContextOptions<PriceDbContext> options) : DbContext(options)
{
    public DbSet<ArticlePrice> ArticlePrices => Set<ArticlePrice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ArticlePrice>(entity =>
        {
            entity.ToTable("ArticlePrices");

            entity.HasKey(articlePrice => articlePrice.ArticleId);

            entity.Property(articlePrice => articlePrice.ArticleId)
                .ValueGeneratedNever();

            entity.Property(articlePrice => articlePrice.BasicPriceInEuros)
                .HasPrecision(18, 2)
                .IsRequired();
        });
    }
}
