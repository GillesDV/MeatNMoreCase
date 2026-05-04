using Microsoft.OpenApi;

namespace PriceService.Api.Configuration;

public static class SwaggerServiceCollectionExtensions
{
    public static IServiceCollection AddApiSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddOpenApi();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Firebase ID token. Use: Bearer {token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(document =>
            {
                var requirement = new OpenApiSecurityRequirement();
                requirement.Add(new OpenApiSecuritySchemeReference("Bearer", document, null), []);

                return requirement;
            });
        });

        return services;
    }
}
