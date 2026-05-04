using Microsoft.Extensions.DependencyInjection;
using StockService.Infrastructure.Persistence;

namespace StockService.Infrastructure;

public static class StockDatabaseInitializer
{
    public static async Task InitializeStockDatabaseAsync(this IServiceProvider serviceProvider)
    {
        const int maxAttempts = 10;

        // Attempt to create the database, retrying if it fails (e.g., if the database server is not yet available).
        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                await using var scope = serviceProvider.CreateAsyncScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<StockDbContext>();

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
