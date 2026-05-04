using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PriceService.Application;
using PriceService.Infrastructure.Persistence;

namespace PriceService.Infrastructure;

public static class PriceInfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddPriceInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PriceDb");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Configure ConnectionStrings:PriceDb.");
        }

        services.AddDbContext<PriceDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IPriceRepository, SqlPriceService>();
        services.AddScoped<IPriceService, Application.PriceService>();

        return services;
    }
}
