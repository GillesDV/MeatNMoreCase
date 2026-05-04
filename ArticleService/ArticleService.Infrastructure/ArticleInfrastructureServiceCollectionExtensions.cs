using ArticleService.Application.Interfaces;
using ArticleService.Infrastructure.Messaging;
using ArticleService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArticleService.Infrastructure;

public static class ArticleInfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddArticleInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ArticleDb");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Configure ConnectionStrings:ArticleDb.");
        }

        services.AddDbContext<ArticleDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IArticleRepository, SqlArticleService>();
        services.AddScoped<IArticleCreatedEventPublisher, ArticleCreatedEventPublisher>();
        services.AddScoped<IArticleService, Application.ArticleService>();

        return services;
    }
}
