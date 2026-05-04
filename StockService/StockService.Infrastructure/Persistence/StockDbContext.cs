using Microsoft.EntityFrameworkCore;
using StockService.Domain.Entity;

namespace StockService.Infrastructure.Persistence;

public sealed class StockDbContext(DbContextOptions<StockDbContext> options) : DbContext(options)
{
    public DbSet<StockItem> StockInfos => Set<StockItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StockItem>(entity =>
        {
            entity.ToTable("StockInfos");

            entity.HasKey(stockInfo => stockInfo.ArticleId);

            entity.Property(stockInfo => stockInfo.ArticleId)
                .ValueGeneratedNever();

            entity.Property(stockInfo => stockInfo.Quantity)
                .IsRequired();

            entity.Property(stockInfo => stockInfo.Location)
            // Should this be a number? Depends on business rules (aka, how likely are we to get a new Location and what kind of names would it get? Should be clear & future-proof regardless)
                .HasConversion<string>() 
                .HasMaxLength(32)
                .IsRequired();
        });
    }
}
