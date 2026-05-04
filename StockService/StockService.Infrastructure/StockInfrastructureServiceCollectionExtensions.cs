using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockService.Application.Interfaces;
using StockService.Infrastructure.Persistence;

namespace StockService.Infrastructure;

public static class StockInfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddStockInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("StockDb");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Configure ConnectionStrings:StockDb.");
        }

        services.AddDbContext<StockDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IStockRepository, SqlStockService>();
        services.AddScoped<IStockService, Application.StockService>();

        return services;
    }
}
