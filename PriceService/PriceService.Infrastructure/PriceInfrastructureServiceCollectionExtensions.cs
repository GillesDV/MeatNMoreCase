using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PriceService.Application.Interfaces;
using PriceService.Application.Proxies.ArticleService;
using PriceService.Application.Proxies.StockService;
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

        services.AddHttpContextAccessor();
        services.AddTransient<AuthorizationHeaderForwardingHandler>();
        services.AddScoped<IPriceRepository, SqlPriceService>();
        services.AddScoped<IPriceService, Application.PriceService>();
        services.AddArticleServiceClient(configuration);
        services.AddStockServiceClient(configuration);

        return services;
    }

    private static IServiceCollection AddArticleServiceClient(this IServiceCollection services, IConfiguration configuration)
    {
        var articleServiceUrl = configuration["ServiceUrls:ArticleService"];

        if (string.IsNullOrWhiteSpace(articleServiceUrl))
        {
            throw new InvalidOperationException("Configure ServiceUrls:ArticleService.");
        }

        services.AddHttpClient<IArticleServiceClient, ArticleServiceClient>(client =>
            client.BaseAddress = new Uri(articleServiceUrl))
            .AddHttpMessageHandler<AuthorizationHeaderForwardingHandler>();

        return services;
    }

    private static IServiceCollection AddStockServiceClient(this IServiceCollection services, IConfiguration configuration)
    {
        var stockServiceUrl = configuration["ServiceUrls:StockService"];

        if (string.IsNullOrWhiteSpace(stockServiceUrl))
        {
            throw new InvalidOperationException("Configure ServiceUrls:StockService.");
        }

        services.AddHttpClient<IStockServiceClient, StockServiceClient>(client =>
            client.BaseAddress = new Uri(stockServiceUrl))
            .AddHttpMessageHandler<AuthorizationHeaderForwardingHandler>();

        return services;
    }
}
