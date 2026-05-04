using Microsoft.EntityFrameworkCore;
using StockService.Domain.Entity;

namespace StockService.Infrastructure.Persistence;

public sealed class StockDbContext(DbContextOptions<StockDbContext> options) : DbContext(options)
{
    public DbSet<StockInfo> StockInfos => Set<StockInfo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StockInfo>(entity =>
        {
            entity.ToTable("StockInfos");

            entity.HasKey(stockInfo => stockInfo.ArticleId);

            entity.Property(stockInfo => stockInfo.ArticleId)
                .ValueGeneratedNever();

            entity.Property(stockInfo => stockInfo.Voorraad)
                .IsRequired();

            entity.Property(stockInfo => stockInfo.Locatie)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();
        });
    }
}
