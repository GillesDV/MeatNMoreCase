using ArticleService.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace ArticleService.Infrastructure.Persistence;

public sealed class ArticleDbContext(DbContextOptions<ArticleDbContext> options) : DbContext(options)
{
    public DbSet<Article> Articles => Set<Article>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.ToTable("Articles");

            entity.HasKey(article => article.ArticleId);

            entity.Property(article => article.ArticleId)
                .ValueGeneratedOnAdd();

            entity.Property(article => article.Omschrijving)
                .HasMaxLength(500);

            entity.Property(article => article.Eenheid)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();
        });
    }
}
