using ArticleService.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace ArticleService.Infrastructure;

public static class ArticleDatabaseInitializer
{
    public static async Task InitializeArticleDatabaseAsync(this IServiceProvider serviceProvider)
    {
        const int maxAttempts = 10;


        // Attempt to create the database, retrying if it fails (e.g., if the database server is not yet available).
        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                await using var scope = serviceProvider.CreateAsyncScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ArticleDbContext>();

                await dbContext.Database.EnsureCreatedAsync();
                return;
            }
            catch when (attempt < maxAttempts)
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
    }
}
